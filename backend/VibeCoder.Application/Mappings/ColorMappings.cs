namespace VibeCoder.Application.Mappings;

public static class ColorMappings
{
    private static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
    {
        // Polish abbreviations → full market names
        { "j. szary", "Light Grey" },
        { "c. szary", "Dark Grey" },
        { "j. niebieski", "Light Blue" },
        { "c. niebieski", "Dark Blue" },
        { "j. zielony", "Light Green" },
        { "c. zielony", "Dark Green" },
        { "j. brązowy", "Light Brown" },
        { "c. brązowy", "Dark Brown" },
        { "j. różowy", "Light Pink" },
        { "c. różowy", "Dark Pink" },
        { "j. fioletowy", "Light Purple" },
        { "c. fioletowy", "Dark Purple" },
        { "szar.", "Grey" },
        { "czar.", "Black" },
        { "nieb.", "Blue" },
        { "ziel.", "Green" },
        { "czer.", "Red" },
        { "biał.", "White" },
        { "brąz.", "Brown" },
        { "żółt.", "Yellow" },
        { "pomar.", "Orange" },
        { "fiol.", "Purple" },
        { "różow.", "Pink" },
        { "beż.", "Beige" },
        { "sreb.", "Silver" },
        { "złot.", "Gold" },
        // Full Polish color names
        { "czarny", "Black" },
        { "biały", "White" },
        { "szary", "Grey" },
        { "niebieski", "Blue" },
        { "czerwony", "Red" },
        { "zielony", "Green" },
        { "brązowy", "Brown" },
        { "żółty", "Yellow" },
        { "pomarańczowy", "Orange" },
        { "fioletowy", "Purple" },
        { "różowy", "Pink" },
        { "beżowy", "Beige" },
        { "beż", "Beige" },
        { "srebrny", "Silver" },
        { "złoty", "Gold" },
    };

    public static string Normalize(string? rawColor)
    {
        if (string.IsNullOrWhiteSpace(rawColor))
            return string.Empty;

        var trimmed = rawColor.Trim();

        if (Map.TryGetValue(trimmed, out var mapped))
            return mapped;

        // Capitalize first letter if no mapping found
        return char.ToUpper(trimmed[0]) + trimmed[1..];
    }
}
