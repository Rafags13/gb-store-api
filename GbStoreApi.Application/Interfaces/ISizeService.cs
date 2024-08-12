using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Sizes;

namespace GbStoreApi.Application.Interfaces
{
    public interface ISizeService
    {
        ResponseDto<IEnumerable<DisplaySizeDto>> GetAll();
        ResponseDto<DisplaySizeDto> GetById(int id);
        ResponseDto<DisplaySizeDto> GetByName(string sizeName);
        ResponseDto<DisplaySizeDto> Create(string sizeName);
        ResponseDto<DisplaySizeDto> Update(UpdateSizeDto updateSizeDto);
        ResponseDto<DisplaySizeDto> Delete(int id);
    }
}
