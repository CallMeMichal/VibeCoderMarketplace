namespace VibeCoder.Application.DTOs;

public class CleanedProductDto
{
    public string Sku { get; set; } = string.Empty;
    public string OriginalName { get; set; } = string.Empty;
    public string AllegroTitle { get; set; } = string.Empty;
    public string CleanDescription { get; set; } = string.Empty;
    public string Dimensions { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public string? Ean { get; set; }
}
