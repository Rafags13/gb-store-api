using GbStoreApi.Domain.Dto;

namespace GbStoreApi.Application.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetAll();
        CategoryDto? GetById(int id);
        CategoryDto? GetByName(string name);
        CategoryDto Create(string categoryName);
        CategoryDto Update(UpdateCategoryDto updateCategoryDto);
        CategoryDto Delete(int id);
    }
}
