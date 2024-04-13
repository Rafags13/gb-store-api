using Amazon.S3.Model;
using GbStoreApi.Application.Exceptions;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Data.Extensions;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            if (_unitOfWork.Save() < 1)
            {
                throw new CantCreateProductException("Não foi possível criar o produto informado.");
            }

            var currentProductId = _unitOfWork.Product.FindOne(x => x.Name == createProductDto.Name).Id;

            var newStockToProduct = new CreateStockWithIdDto { ProductId = currentProductId, Variants = createProductDto.Stock };

            var createStockSuccess = _stockService.CreateMultipleStock(newStockToProduct) > 0;
            if (!createStockSuccess) throw new CantCreateProductException("Não foi possível criar os estoques. Fale com o administrador do sistema.");

            var picturesName = await _fileService.CreateMultipleFiles(createProductDto.Photos);

            var picturesToCreate = picturesName.Select(name =>
                new CreatePictureDto
                {
                    Name = name,
                });

            if (!picturesToCreate.Any()) return false;

            var picturesWithProductId = new CreateMultiplePicturesDto
            {
                Pictures = picturesToCreate,
                ProductId = currentProductId
            };

            _pictureService.CreateMultiplePictures(picturesWithProductId);

            return true;
        }

        public IEnumerable<DisplayProductDto>? GetAll()
        {
            var productsReference =
                _unitOfWork.Product
                .GetAll()
                .Include(picture => picture.Pictures)
                .Include(stock => stock.Stocks)
                .ThenInclude(color => color.Color)
                .Include(stock => stock.Stocks)
                .ThenInclude(size => size.Size).Take(25).ToList();

            var products =
                productsReference
                .Select(product => new DisplayProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    RealPrice = product.UnitaryPrice,
                    DiscountPercent = product.DiscountPercent,
                    PriceWithDiscount = product.UnitaryPrice / (decimal)(1 - product.DiscountPercent ?? 0),
                    PhotoUrlId = product.Pictures.FirstOrDefault()?.Name ?? "",
                    VariantNames = product.Stocks.Select(stocks => stocks.Color!.Name)
                                                 .Concat(product.Stocks.Select(color => color.Size!.Name)).Distinct()
                });

            return products;
        }

        public IEnumerable<DisplayProductDto> GetByFilters(CatalogFilterDto filters)
        {

            var productsFiltered =
                _unitOfWork.Product
                .GetAll()
                .Include(x => x.Stocks)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Pictures)
                .Include(x => x.Stocks)
                .ThenInclude(x => x.Color)
                .Include(x => x.Stocks)
                .ThenInclude(x => x.Size)
                .FilterByCategoryIfWasInformed(filters.Category)
                .FilterByColorsIfWereInformed(filters.Cores)
                .FilterBySizesIfWereInformed(filters.Tamanhos)
                .ToList()
                ;

                return CreateListOfDtos(productsFiltered);
        }
        
        private static IEnumerable<DisplayProductDto> CreateListOfDtos(IEnumerable<Product> products)
        {
            return products.ToList().Select(product => new DisplayProductDto
            {
                Id = product.Id,
                Name = product.Name,
                RealPrice = product.UnitaryPrice,
                DiscountPercent = product.DiscountPercent,
                PriceWithDiscount = product.UnitaryPrice / (decimal)(1 - product.DiscountPercent ?? 0),
                PhotoUrlId = product.Pictures.FirstOrDefault()?.Name ?? "",
                VariantNames = product.Stocks.Select(stocks => stocks.Color!.Name)
                                                 .Concat(product.Stocks.Select(color => color.Size!.Name)).Distinct()
            });
        }

        public DisplayVariantsDto? GetCurrentVariants()
        {
            var colors = _unitOfWork.Color.GetAll().Select(x => new DisplayColorDto { Id = x.Id, Name = x.Name });
            var sizes = _unitOfWork.Size.GetAll().Select(x => new DisplaySizeDto { Id = x.Id, Name = x.Name });

            var currentVariants = new DisplayVariantsDto { Colors = colors, Sizes = sizes };

            return currentVariants;
        }
    }
}
