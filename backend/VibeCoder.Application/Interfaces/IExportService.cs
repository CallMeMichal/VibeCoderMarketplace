using VibeCoder.Application.DTOs;

namespace VibeCoder.Application.Interfaces;

public interface IExportService
{
    byte[] ExportToXlsx(List<CleanedProductDto> products);
    byte[] ExportToCsv(List<CleanedProductDto> products);
}
