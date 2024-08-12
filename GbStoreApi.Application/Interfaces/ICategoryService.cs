using GbStoreApi.Domain.Dto.Categories;
using GbStoreApi.Domain.Dto.Generic;

namespace GbStoreApi.Application.Interfaces
{
    public interface ICategoryService
    {
        ResponseDto<IEnumerable<DisplayCategoryDto>> GetAll();
        ResponseDto<DisplayCategoryDto> GetById(int id);
        ResponseDto<DisplayCategoryDto> GetByName(string name);
        ResponseDto<DisplayCategoryDto> Create(string categoryName);
        ResponseDto<DisplayCategoryDto> Update(UpdateCategoryDto updateCategoryDto);
        ResponseDto<DisplayCategoryDto> Delete(int id);
    }
}
