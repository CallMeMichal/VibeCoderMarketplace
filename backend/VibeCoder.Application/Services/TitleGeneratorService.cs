using VibeCoder.Application.Interfaces;
using VibeCoder.Application.Mappings;

namespace VibeCoder.Application.Services;

public class TitleGeneratorService : ITitleGeneratorService
{
    public string GenerateAllegroTitle(string productName, string? dimensions, string? color)
    {
        // Build a sales-oriented Allegro title (max 75 chars)
        // Pattern: "Dywanik Łazienkowy Belweder {Dimensions} {Color} Antypoślizgowy"
        var baseName = CleanBaseName(productName);
        var colorFull = ColorMappings.Normalize(color);

        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(baseName))
            parts.Add(baseName);

        if (!string.IsNullOrWhiteSpace(dimensions))
            parts.Add(dimensions);

        if (!string.IsNullOrWhiteSpace(colorFull))
            parts.Add(colorFull);

        // Add selling keyword if there's room
        var title = string.Join(" ", parts);
        const string suffix = " Antypoślizgowy";
        if (title.Length + suffix.Length <= 75)
            title += suffix;

        if (title.Length > 75)
            title = title[..75].TrimEnd();

        return title;
    }

    private static string CleanBaseName(string name)
    {
        // Remove dimension info and color shorthand from original name
        // "Dyw. Łazienkowy Belweder 040*060cm czarny" → "Dywanik Łazienkowy Belweder"
        var cleaned = System.Text.RegularExpressions.Regex.Replace(
            name, @"\d{2,4}\s*[*xX×]\s*\d{2,4}\s*(?:cm)?.*$", "", 
            System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();

        // Expand common abbreviations
        cleaned = cleaned.Replace("Dyw.", "Dywanik");

        return cleaned;
    }
}
