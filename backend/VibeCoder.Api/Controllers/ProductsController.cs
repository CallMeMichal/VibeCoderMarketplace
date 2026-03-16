using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VibeCoder.Application.DTOs;
using VibeCoder.Application.Interfaces;

namespace VibeCoder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductCleaningService _cleaningService;
    private readonly IExportService _exportService;

    private static List<CleanedProductDto> _cleanedProducts = [];

    public ProductsController(IProductCleaningService cleaningService, IExportService exportService)
    {
        _cleaningService = cleaningService;
        _exportService = exportService;
    }

    /// <summary>
    /// Upload and process a dirty JSON export file.
    /// </summary>
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file provided.");

        using var stream = file.OpenReadStream();
        var rawProducts = await JsonSerializer.DeserializeAsync<List<RawProductDto>>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (rawProducts is null || rawProducts.Count == 0)
            return BadRequest("File contains no products.");

        _cleanedProducts = _cleaningService.CleanProducts(rawProducts);
        return Ok(_cleanedProducts);
    }

    /// <summary>
    /// Get all cleaned products.
    /// </summary>
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_cleanedProducts);
    }

    /// <summary>
    /// Export cleaned products as XLSX or CSV.
    /// </summary>
    [HttpGet("export")]
    public IActionResult Export([FromQuery] string format = "xlsx")
    {
        if (_cleanedProducts.Count == 0)
            return NotFound("No processed products found. Upload a file first.");

        return format.ToLowerInvariant() switch
        {
            "xlsx" => File(_exportService.ExportToXlsx(_cleanedProducts),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "products.xlsx"),
            "csv" => File(_exportService.ExportToCsv(_cleanedProducts),
                "text/csv",
                "products.csv"),
            _ => BadRequest("Supported formats: xlsx, csv")
        };
    }
}
