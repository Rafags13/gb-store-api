using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BrandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DisplayBrandDto Create(string brandName)
        {
            var newBrand = new Brand { Name = brandName};
            if (BrandExists(brandName)) throw new ArgumentException("A marca informada já existe no sistema.");

            _unitOfWork.Brand.Add(newBrand);
            _unitOfWork.Save();

            var currentBrand = GetByName(newBrand.Name);

            return currentBrand;
        }

        private bool BrandExists(string brandName)
        {
            return GetByName(brandName) != null;
        }

        public IEnumerable<DisplayBrandDto> GetAll()
        {
            return _unitOfWork.Brand.GetAll().Select(x => new DisplayBrandDto { Id = x.Id, Name = x.Name });
        }

        public DisplayBrandDto? GetById(int id)
        {
            var currentBrand = _unitOfWork.Brand.GetById(id);

            if (currentBrand is null) return null;

            var displayBrand = new DisplayBrandDto { Id = currentBrand.Id, Name = currentBrand.Name };

            return displayBrand;
        }

        public DisplayBrandDto? GetByName(string brandName)
        {
            var currentBrand = _unitOfWork.Brand.FindOne(x => x.Name == brandName);

            if (currentBrand is null) return null;

            var displayBrand = new DisplayBrandDto { Id = currentBrand.Id, Name = currentBrand.Name };

            return displayBrand;
        }
    }
}
