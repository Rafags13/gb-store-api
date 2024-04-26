using AutoMapper;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Categories;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;

namespace GbStoreApi.Application.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ResponseDto<IEnumerable<DisplayCategoryDto>> GetAll()
        {
            var categories = _unitOfWork.Category.GetAll().Select(category => _mapper.Map<DisplayCategoryDto>(category));

            return new ResponseDto<IEnumerable<DisplayCategoryDto>>(categories, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayCategoryDto> GetById(int id)
        {
            var currentCategory = _unitOfWork.Category.FindOne(x => x.Id == id);

            if (currentCategory is null)
                return new ResponseDto<DisplayCategoryDto>(StatusCodes.Status404NotFound, "Não existe nenhuma categoria com esse Id.");

            var category = _mapper.Map<DisplayCategoryDto>(currentCategory);

            return new ResponseDto<DisplayCategoryDto>(category, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayCategoryDto> Create(string categoryName)
        {
            if(_unitOfWork.Category.Contains(x => x.Name == categoryName))
                return new ResponseDto<DisplayCategoryDto>(StatusCodes.Status400BadRequest, "Uma categoria com esse nome já existe.");

            var newCategory = new Category { Name = categoryName };

            _unitOfWork.Category.Add(newCategory);
            _unitOfWork.Save();

            var response = GetByName(categoryName);

            if (response.StatusCode != StatusCodes.Status200OK || response.Value is null)
                return new ResponseDto<DisplayCategoryDto>(response.StatusCode, response.Message!);

            return new ResponseDto<DisplayCategoryDto>(response.Value, StatusCodes.Status201Created);
        }


        public ResponseDto<DisplayCategoryDto> GetByName(string categoryName)
        {
            var currentCategory = _unitOfWork.Category.FindOne(x => x.Name == categoryName);

            if (currentCategory is null)
                return new ResponseDto<DisplayCategoryDto>(StatusCodes.Status404NotFound, "Não existe nenhuma categoria com esse nome.");

            var category = _mapper.Map<DisplayCategoryDto>(currentCategory);

            return new ResponseDto<DisplayCategoryDto>(category, StatusCodes.Status200OK);
        }
        public ResponseDto<DisplayCategoryDto> Update(UpdateCategoryDto updateCategoryDto)
        {
            var currentCategory = _unitOfWork.Category.GetOneByName(updateCategoryDto.OldCategoryName);

            if (currentCategory is null)
                return new ResponseDto<DisplayCategoryDto>(StatusCodes.Status404NotFound, "Não existe nenhuma categoria com esse nome");

            currentCategory.Name = updateCategoryDto.NewCategoryName;

            var updatedCategory = _unitOfWork.Category.Update(currentCategory);
            _unitOfWork.Save();
            var categoryToResponse = _mapper.Map<DisplayCategoryDto>(updatedCategory);

            return new ResponseDto<DisplayCategoryDto>(categoryToResponse, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayCategoryDto> Delete(int id)
        {
            var currentCategory = _unitOfWork.Category.GetOneById(id);

            if(currentCategory is null)
                return new ResponseDto<DisplayCategoryDto>(StatusCodes.Status404NotFound, "Não existe nenhuma categoria com esse nome");

            var deletedCategory = _unitOfWork.Category.Remove(currentCategory);
            _unitOfWork.Save();
            var categoryToResponse = _mapper.Map<DisplayCategoryDto>(deletedCategory);

            return new ResponseDto<DisplayCategoryDto>(categoryToResponse, StatusCodes.Status200OK);
        }
    }
}
