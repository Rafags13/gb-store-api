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
        private readonly IFileService _fileService;
        private readonly IPictureService _pictureService;
        public ProductService(
            IUnitOfWork unitOfWork,
            IStockService stockService,
            IFileService fileService,
            IPictureService pictureService
           )
        {
            _unitOfWork = unitOfWork;
            _stockService = stockService;
            _fileService = fileService;
            _pictureService = pictureService;
        }
        public async Task<bool> CreateProduct(CreateProductDto createProductDto)
        {
            //var successCreateImages = await _fileService.CreateMultipleFiles(createProductDto.Files);

            //if (!successCreateImages) throw new CantCreateProductException("Alguma das fotos falhou ao ser incluida.");

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

            var createStockSuccess = _stockService.CreateMultipleStock(newStockToProduct) > 0;

            //var picturesToCreate = createProductDto.Files.Select(x =>
            //    new CreatePictureDto
            //    {
            //        Name = x.Name
            //    });

            //if (!picturesToCreate.Any()) return false;

            //var picturesWithProductId = new CreateMultiplePicturesDto {
            //    Pictures = picturesToCreate, ProductId = currentProductId 
            //};

            //_pictureService.CreateMultiplePictures(picturesWithProductId);

            return true;
        }
    }
}
