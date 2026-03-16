using System.Text.Json.Serialization;

namespace VibeCoder.Application.DTOs;

public class RawProductDto
{
    [JsonPropertyName("NAZWA ORG")]
    public string? Name { get; set; }

    [JsonPropertyName("SKU")]
    public string? Sku { get; set; }

    [JsonPropertyName("Cena")]
    public string? Price { get; set; }

    [JsonPropertyName("Opis ofe")]
    public string? Description { get; set; }

    [JsonPropertyName("Stany")]
    public object? Stock { get; set; }

    [JsonPropertyName("EAN")]
    public string? Ean { get; set; }
}
