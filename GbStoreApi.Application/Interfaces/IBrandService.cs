using GbStoreApi.Domain.Dto.Brands;
using GbStoreApi.Domain.Dto.Generic;

namespace GbStoreApi.Application.Interfaces
{
    public interface IBrandService
    {
        ResponseDto<IEnumerable<DisplayBrandDto>> GetAll();
        DisplayBrandDto? GetById(int id);
        DisplayBrandDto? GetByName(string brandName);
        DisplayBrandDto Create(string brandName);
    }
}
