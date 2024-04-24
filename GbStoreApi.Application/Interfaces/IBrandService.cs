using GbStoreApi.Domain.Dto.Brands;

namespace GbStoreApi.Application.Interfaces
{
    public interface IBrandService
    {
        IEnumerable<DisplayBrandDto> GetAll();
        DisplayBrandDto? GetById(int id);
        DisplayBrandDto? GetByName(string brandName);
        DisplayBrandDto Create(string brandName);
    }
}
