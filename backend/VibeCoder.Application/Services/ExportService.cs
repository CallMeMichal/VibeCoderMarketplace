using System.Globalization;
using System.Text;
using ClosedXML.Excel;
using VibeCoder.Application.DTOs;
using VibeCoder.Application.Interfaces;

namespace VibeCoder.Application.Services;

public class ExportService : IExportService
{
    public byte[] ExportToXlsx(List<CleanedProductDto> products)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Products");

        // Headers
        var headers = new[] { "SKU", "Allegro Title", "Description", "Dimensions", "Color", "Price", "Stock", "EAN" };
        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
        }

        // Data rows
        for (int row = 0; row < products.Count; row++)
        {
            var p = products[row];
            worksheet.Cell(row + 2, 1).Value = p.Sku;
            worksheet.Cell(row + 2, 2).Value = p.AllegroTitle;
            worksheet.Cell(row + 2, 3).Value = p.CleanDescription;
            worksheet.Cell(row + 2, 4).Value = p.Dimensions;
            worksheet.Cell(row + 2, 5).Value = p.Color;
            worksheet.Cell(row + 2, 6).Value = p.Price?.ToString("F2", CultureInfo.InvariantCulture) ?? "";
            worksheet.Cell(row + 2, 7).Value = p.Stock?.ToString() ?? "";
            worksheet.Cell(row + 2, 8).Value = p.Ean ?? "";
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportToCsv(List<CleanedProductDto> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("SKU;Allegro Title;Description;Dimensions;Color;Price;Stock;EAN");

        foreach (var p in products)
        {
            sb.AppendLine(string.Join(";",
                Escape(p.Sku),
                Escape(p.AllegroTitle),
                Escape(p.CleanDescription),
                Escape(p.Dimensions),
                Escape(p.Color),
                p.Price?.ToString("F2", CultureInfo.InvariantCulture) ?? "",
                p.Stock?.ToString() ?? "",
                Escape(p.Ean ?? "")));
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
    }

    private static string Escape(string value)
    {
        if (value.Contains(';') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }
}
