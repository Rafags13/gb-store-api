using AutoMapper;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Brands;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;

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

        public ResponseDto<DisplayBrandDto> Create(string brandName)
        {
            var newBrand = new Brand { Name = brandName };
            if (BrandExists(brandName)) throw new ArgumentException("A marca informada já existe no sistema.");

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
            var brands = _unitOfWork.Brand.GetAll().Select(brand => _mapper.Map<DisplayBrandDto>(brand));

            if(!brands.Any())
                return new ResponseDto<IEnumerable<DisplayBrandDto>>(brands, StatusCodes.Status400BadRequest, "Nenhuma marca foi encontrada.");

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
    }
}
