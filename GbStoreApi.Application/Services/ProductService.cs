using GbStoreApi.Application.Exceptions;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStockService _stockService;
        public ProductService(
            IUnitOfWork unitOfWork,
            IStockService stockService
           )
        {
            _unitOfWork = unitOfWork;
            _stockService = stockService;
        }
        public bool CreateProduct(CreateProductDto createProductDto)
        {
            var newProduct = new Product { 
                Name = createProductDto.Name,
                Description = createProductDto.Description ?? "",
                DiscountPercent = createProductDto.DiscountPercent ?? 0.0f,
                QuotasNumber = createProductDto.QuotasNumber ?? 0,
                UnitaryPrice = createProductDto.UnitaryPrice,
                CategoryId = createProductDto.CategoryId,
                BrandId = createProductDto.BrandId,
            };

            _unitOfWork.Product.Add(newProduct);
            if(_unitOfWork.Save() < 1)
            {
                throw new CantCreateProductException("Não foi possível criar o produto informado.");
            }

            var currentProductId = _unitOfWork.Product.FindOne(x => x.Name == createProductDto.Name).Id;
            var newStockToProduct = new CreateStockWithIdDto { ProductId = currentProductId, Variants = createProductDto.Stocks };

            var success = _stockService.CreateMultipleStock(newStockToProduct) > 0;

            return success;
        }
    }
}
