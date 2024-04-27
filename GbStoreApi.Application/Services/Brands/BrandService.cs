using AutoMapper;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Brands;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GbStoreApi.Application.Services.Brands
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BrandService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region [CRUD]
        public ResponseDto<DisplayBrandDto> Create(string brandName)
        {
            var newBrand = new Brand { Name = brandName };
            if (BrandExists(brandName))
                return new ResponseDto<DisplayBrandDto>(StatusCodes.Status400BadRequest, "A marca informada já existe no sistema.");

            _unitOfWork.Brand.Add(newBrand);
            _unitOfWork.Save();

            var response = GetByName(newBrand.Name);

            if (response.StatusCode != StatusCodes.Status200OK || response.Value is null)
                return new ResponseDto<DisplayBrandDto>(response.StatusCode, response.Message!);

            return new ResponseDto<DisplayBrandDto>(response.Value, StatusCodes.Status200OK);
        }

        private bool BrandExists(string brandName)
        {
            return _unitOfWork.Brand.Contains(x => x.Name == brandName);
        }

        public ResponseDto<IEnumerable<DisplayBrandDto>> GetAll()
        {
            var brands = _unitOfWork.Brand.GetAll().Select(brand => _mapper.Map<DisplayBrandDto>(brand))
                ?? Enumerable.Empty<DisplayBrandDto>().AsQueryable();

            return new ResponseDto<IEnumerable<DisplayBrandDto>>(brands, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayBrandDto> GetById(int id)
        {
            var currentBrand = _unitOfWork.Brand.GetById(id);

            if (currentBrand is null)
                return new ResponseDto<DisplayBrandDto>(StatusCodes.Status404NotFound, "Não existe nehuma marca com esse Id.");

            var brand = _mapper.Map<DisplayBrandDto>(currentBrand);

            return new ResponseDto<DisplayBrandDto>(brand, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayBrandDto> GetByName(string brandName)
        {
            var currentBrand = _unitOfWork.Brand.FindOne(x => x.Name == brandName);

            if (currentBrand is null)
                return new ResponseDto<DisplayBrandDto>(StatusCodes.Status404NotFound, "Não existe nehuma marca com esse Id.");

            var brand = _mapper.Map<DisplayBrandDto>(currentBrand);

            return new ResponseDto<DisplayBrandDto>(brand, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayBrandDto> Update(UpdateBrandDto updateBrandDto)
        {
            var currentBrand = _unitOfWork.Brand.GetByName(updateBrandDto.OldBrandName);

            if (currentBrand is null)
                return new ResponseDto<DisplayBrandDto>(StatusCodes.Status404NotFound, "Não existe nenhuma marca com esse nome");
            currentBrand.Name = updateBrandDto.OldBrandName;

            var updatedBrand = _unitOfWork.Brand.Update(currentBrand);
            var brandToResponse = _mapper.Map<DisplayBrandDto>(updatedBrand);

            return new ResponseDto<DisplayBrandDto>(brandToResponse, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayBrandDto> Delete(int id)
        {
            var currentBrand = 
                _unitOfWork
                .Brand
                .GetByIdAndReturnQueryable(id)
                .Include(x => x.Products)
                .FirstOrDefault();

            if (currentBrand is null)
                return new ResponseDto<DisplayBrandDto>(StatusCodes.Status404NotFound, "Não existe nenhuma marca com esse Id.");

            if(currentBrand.Products is null)
                return new ResponseDto<DisplayBrandDto>(StatusCodes.Status404NotFound,
                    "Não é possível excluir essa marca, pois ela está ligada a um produto. Exclua essa ligação para poder removê-la.");

            var deletedBrand = _unitOfWork.Brand.Remove(currentBrand);
            var brandToResponse = _mapper.Map<DisplayBrandDto>(deletedBrand);

            return new ResponseDto<DisplayBrandDto>(brandToResponse, StatusCodes.Status200OK);
        }
        #endregion
    }
}
