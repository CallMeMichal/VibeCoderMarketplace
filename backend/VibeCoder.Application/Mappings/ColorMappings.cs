namespace VibeCoder.Application.Mappings;

public static class ColorMappings
{
    private static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
    {
        // Polish abbreviations → full Polish names
        { "j. szary", "Jasnoszary" },
        { "c. szary", "Ciemnoszary" },
        { "j. niebieski", "Jasnoniebieski" },
        { "c. niebieski", "Ciemnoniebieski" },
        { "j. zielony", "Jasnozielony" },
        { "c. zielony", "Ciemnozielony" },
        { "j. brązowy", "Jasnobrązowy" },
        { "c. brązowy", "Ciemnobrązowy" },
        { "j. różowy", "Jasnoróżowy" },
        { "c. różowy", "Ciemnoróżowy" },
        { "j. fioletowy", "Jasnofioletowy" },
        { "c. fioletowy", "Ciemnofioletowy" },
        { "szar.", "Szary" },
        { "czar.", "Czarny" },
        { "nieb.", "Niebieski" },
        { "ziel.", "Zielony" },
        { "czer.", "Czerwony" },
        { "biał.", "Biały" },
        { "brąz.", "Brązowy" },
        { "żółt.", "Żółty" },
        { "pomar.", "Pomarańczowy" },
        { "fiol.", "Fioletowy" },
        { "różow.", "Różowy" },
        { "beż.", "Beżowy" },
        { "sreb.", "Srebrny" },
        { "złot.", "Złoty" },
        // Full Polish color names (normalize capitalization)
        { "czarny", "Czarny" },
        { "biały", "Biały" },
        { "szary", "Szary" },
        { "niebieski", "Niebieski" },
        { "czerwony", "Czerwony" },
        { "zielony", "Zielony" },
        { "brązowy", "Brązowy" },
        { "żółty", "Żółty" },
        { "pomarańczowy", "Pomarańczowy" },
        { "fioletowy", "Fioletowy" },
        { "różowy", "Różowy" },
        { "beżowy", "Beżowy" },
        { "beż", "Beżowy" },
        { "srebrny", "Srebrny" },
        { "złoty", "Złoty" },
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
