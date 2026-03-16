using System.Text.Json;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using VibeCoder.Application.DTOs;
using VibeCoder.Application.Interfaces;
using VibeCoder.Application.Mappings;

namespace VibeCoder.Application.Services;

public partial class ProductCleaningService : IProductCleaningService
{
    private readonly ITitleGeneratorService _titleGenerator;

    public ProductCleaningService(ITitleGeneratorService titleGenerator)
    {
        _titleGenerator = titleGenerator;
    }

    public List<CleanedProductDto> CleanProducts(List<RawProductDto> rawProducts)
    {
        return rawProducts.Select(CleanSingle).ToList();
    }

    private CleanedProductDto CleanSingle(RawProductDto raw)
    {
        var name = raw.Name?.Trim() ?? string.Empty;
        var dimensions = ExtractDimensions(name);
        var color = ExtractColor(name);

        return new CleanedProductDto
        {
            Sku = raw.Sku?.Trim() ?? string.Empty,
            OriginalName = name,
            AllegroTitle = _titleGenerator.GenerateAllegroTitle(name, dimensions, color),
            CleanDescription = CleanDescription(raw.Description),
            Dimensions = dimensions,
            Color = ColorMappings.Normalize(color),
            Price = ParsePrice(raw.Price),
            Stock = ParseStock(raw.Stock),
            Ean = ParseEan(raw.Ean)
        };
    }

    private static string ExtractDimensions(string name)
    {
        // Match patterns like "040*060cm", "050*080cm", "40x60"
        var match = DimensionsInNameRegex().Match(name);
        if (!match.Success) return string.Empty;

        var w = match.Groups[1].Value.TrimStart('0');
        var h = match.Groups[2].Value.TrimStart('0');
        if (string.IsNullOrEmpty(w)) w = "0";
        if (string.IsNullOrEmpty(h)) h = "0";
        return $"{w} x {h} cm";
    }

    private static string ExtractColor(string name)
    {
        // Color is the last segment after the last dimension pattern
        var match = ColorInNameRegex().Match(name);
        return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
    }

    private static decimal? ParsePrice(string? price)
    {
        if (string.IsNullOrWhiteSpace(price)) return null;

        // Remove "PLN", whitespace, and normalize comma to dot
        var cleaned = price
            .Replace("PLN", "", StringComparison.OrdinalIgnoreCase)
            .Replace(",", ".")
            .Trim();

        return decimal.TryParse(cleaned, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var result) ? result : null;
    }

    private static int? ParseStock(object? stock)
    {
        if (stock is null) return null;
        var str = stock.ToString();
        if (string.IsNullOrWhiteSpace(str)) return null;
        return int.TryParse(str, out var result) ? result : null;
    }

    private static string? ParseEan(string? ean)
    {
        if (string.IsNullOrWhiteSpace(ean)) return null;
        // Only return valid numeric EANs
        return EanRegex().IsMatch(ean) ? ean : null;
    }

    private static string CleanDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return string.Empty;

        // Try to extract from Allegro-style JSON sections
        var jsonExtracted = TryExtractFromAllegroJson(description);
        if (jsonExtracted != null)
            return NormalizeWhitespace(jsonExtracted);

        // Strip HTML tags
        var doc = new HtmlDocument();
        doc.LoadHtml(description);
        var text = doc.DocumentNode.InnerText;

        // Decode HTML entities
        text = System.Net.WebUtility.HtmlDecode(text);

        return NormalizeWhitespace(text);
    }

    private static string? TryExtractFromAllegroJson(string input)
    {
        if (!input.TrimStart().StartsWith('{')) return null;

        try
        {
            using var doc = JsonDocument.Parse(input);
            var root = doc.RootElement;

            // Handle Allegro {"sections":[{"items":[{"type":"TEXT","content":"..."}]}]} format
            if (root.TryGetProperty("sections", out var sections))
            {
                var texts = new List<string>();
                foreach (var section in sections.EnumerateArray())
                {
                    if (!section.TryGetProperty("items", out var items)) continue;
                    foreach (var item in items.EnumerateArray())
                    {
                        if (item.TryGetProperty("content", out var content))
                            texts.Add(content.GetString() ?? "");
                    }
                }
                if (texts.Count > 0)
                    return string.Join(" ", texts);
            }

            // Fallback: try common keys
            foreach (var key in new[] { "description", "desc", "text", "content" })
            {
                if (root.TryGetProperty(key, out var prop))
                    return prop.GetString();
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    private static string NormalizeWhitespace(string text)
    {
        return MultipleSpacesRegex().Replace(text, " ").Trim();
    }

    // "040*060cm" or "050*080cm" — digits possibly with leading zeros, separated by * or x
    [GeneratedRegex(@"(\d{2,4})\s*[*xX×]\s*(\d{2,4})\s*(?:cm)?", RegexOptions.IgnoreCase)]
    private static partial Regex DimensionsInNameRegex();

    // Color is everything after the last "cm" or dimension pattern
    [GeneratedRegex(@"\d+\s*[*xX×]\s*\d+\s*(?:cm)?\s+(.+)$", RegexOptions.IgnoreCase)]
    private static partial Regex ColorInNameRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex MultipleSpacesRegex();

    [GeneratedRegex(@"^\d{8,13}$")]
    private static partial Regex EanRegex();
}
