using GbStoreApi.Domain.Dto;

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
