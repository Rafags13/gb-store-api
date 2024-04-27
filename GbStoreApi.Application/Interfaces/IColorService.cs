using GbStoreApi.Domain.Dto.Colors;
using GbStoreApi.Domain.Dto.Generic;

namespace GbStoreApi.Application.Interfaces
{
    public interface IColorService
    {
        ResponseDto<DisplayColorDto> CreateColor(string colorName);
        ResponseDto<IEnumerable<DisplayColorDto>> GetAll();
        ResponseDto<DisplayColorDto> GetByName(string colorName);
        ResponseDto<DisplayColorDto> GetById(int id);
        ResponseDto<DisplayColorDto> Update(UpdateColorDto updateColorDto);
        ResponseDto<DisplayColorDto> Delete(int id);
    }
}
