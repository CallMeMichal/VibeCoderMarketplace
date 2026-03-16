namespace VibeCoder.Application.Interfaces;

public interface ITitleGeneratorService
{
    string GenerateAllegroTitle(string productName, string? dimensions, string? color);
}
