using GbStoreApi.Domain.Dto.Brands;
using GbStoreApi.Domain.Dto.Generic;

namespace GbStoreApi.Application.Interfaces
{
    public interface IBrandService
    {
        ResponseDto<IEnumerable<DisplayBrandDto>> GetAll();
        ResponseDto<DisplayBrandDto> GetById(int id);
        ResponseDto<DisplayBrandDto> GetByName(string brandName);
        ResponseDto<DisplayBrandDto> Create(string brandName);
        ResponseDto<DisplayBrandDto> Update(UpdateBrandDto updateBrandDto);
        ResponseDto<DisplayBrandDto> Delete(int id);
    }
}
