using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public CategoryDto Create(string categoryName)
        {
            var newCategory = new Category { Name = categoryName};

            _unitOfWork.Category.Add(newCategory);
            _unitOfWork.Save();

            var currentCategory = GetByName(categoryName);

            return currentCategory;

        }

        public CategoryDto? GetByName(string  categoryName)
        {
            var currentCategory = _unitOfWork.Category.FindOne(x => x.Name == categoryName);

            if(currentCategory is null)
            {
                return null;
            }

            var categoryDto = new CategoryDto { Name = categoryName, Id = currentCategory.Id };

            return categoryDto;

        }

        public CategoryDto Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CategoryDto> GetAll()
        {
            return _unitOfWork.Category.GetAll().Select(x => new CategoryDto { Id = x.Id, Name = x.Name});
        }

        public CategoryDto? GetById(int id)
        {
            var currentCategory = _unitOfWork.Category.FindOne(x => x.Id == id);

            if(currentCategory is null)
            {
                return null;
            }

            var data = new CategoryDto { Id = currentCategory.Id, Name = currentCategory.Name };

            return data;
        }

        public CategoryDto Update(UpdateCategoryDto updateCategoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
