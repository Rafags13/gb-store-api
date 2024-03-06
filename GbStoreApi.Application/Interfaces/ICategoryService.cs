using GbStoreApi.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
