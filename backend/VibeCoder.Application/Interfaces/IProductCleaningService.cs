using VibeCoder.Application.DTOs;

namespace VibeCoder.Application.Interfaces;

public interface IProductCleaningService
{
    List<CleanedProductDto> CleanProducts(List<RawProductDto> rawProducts);
}
