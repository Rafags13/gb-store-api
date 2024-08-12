using AutoMapper;
using GbStoreApi.Application.Exceptions;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Data.Extensions;
using GbStoreApi.Domain.Dto.Products;
using GbStoreApi.Domain.Dto.Product.Catalogs;
using GbStoreApi.Domain.Dto.Stocks;
using GbStoreApi.Domain.Dto.Stocks.Filters;
using GbStoreApi.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Dto.Pictures;
using GbStoreApi.Domain.Dto.Colors;
using GbStoreApi.Domain.Dto.Sizes;
using GbStoreApi.Domain.Dto.Generic;
using Microsoft.AspNetCore.Http;
using GbStoreApi.Domain.Enums;
using AutoMapper.QueryableExtensions;

namespace GbStoreApi.Application.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStockService _stockService;
        private readonly IFileService _fileService;
        private readonly IPictureService _pictureService;
        private readonly IMapper _mapper;
        public ProductService(
            IUnitOfWork unitOfWork,
            IStockService stockService,
            IFileService fileService,
            IPictureService pictureService,
            IMapper mapper
           )
        {
            _unitOfWork = unitOfWork;
            _stockService = stockService;
            _fileService = fileService;
            _pictureService = pictureService;
            _mapper = mapper;
        }
        public async Task<ResponseDto<bool>> CreateProduct(CreateProductDto createProductDto)
        {
            using var transaction = _unitOfWork.GetContext().BeginTransaction();

            var picturesName = await _fileService.CreateMultipleFiles(createProductDto.Photos);

            if (!picturesName.Any()) return new(StatusCodes.Status400BadRequest, "Não foi possível criar as fotos do produto.");

            var newPictures = _mapper.Map<List<Picture>>(picturesName);

            var newProduct = _mapper.Map<Product>(createProductDto, opt => opt.AfterMap((_, product) =>
            {
                product.Pictures.AddRange(newPictures);
            }));

            _unitOfWork.Product.Add(newProduct);

            if (_unitOfWork.Save() == 0)
                return new(StatusCodes.Status422UnprocessableEntity, "Não foi possível criar os dados do Produto.");

            await transaction.CommitAsync();

            return new(true);
        }

        public ResponseDto<IEnumerable<DisplayProductDto>> GetAll()
        {
            var productsReference =
                _unitOfWork.Product
                .GetAll()
                .Include(picture => picture.Pictures)
                .Include(stock => stock.Stocks)
                .ThenInclude(color => color.Color)
                .Include(stock => stock.Stocks)
                .ThenInclude(size => size.Size)
                .Select(x => _mapper.Map<DisplayProductDto>(x))
                .Paginate();

            if (!productsReference.Any())
                return new(Enumerable.Empty<DisplayProductDto>());

            return new(productsReference);
        }

        public async Task<PaginatedResponseDto<IEnumerable<DisplayProductDto>>> GetByFilters(CatalogFilterDto filters)
        {
               (int Page,
                int PageSize,
                string[]? Sizes,
                string[]? Colors,
                string? OrderBy,
                string? Direction,
                string? ProductName,
                string? Category
                ) = filters;

            var productsFiltered = await _unitOfWork.Product
                .GetAll()
                .WithPictures()
                .WithSizes()
                .WithColors()
                .WithCategories()
                .AsNoTracking()
                .ProjectTo<DisplayProductDto>(_mapper.ConfigurationProvider)
                .FilterByProductNameIfWasInformed(ProductName)
                .FilterByCategoryIfWasInformed(Category)
                .FilterByColorsIfWereInformed(Colors)
                .FilterBySizesIfWereInformed(Sizes)
                .FilterByActiveProducts()
                .OrderBy(Direction, OrderBy)
                .Count(out int count)
                .Paginate(Page, PageSize)
                .ToListAsync();

            return new PaginatedResponseDto<IEnumerable<DisplayProductDto>>(
                productsFiltered,
                Page,
                PageSize,
                count
                );
        }

        public ResponseDto<DisplayVariantsDto?> GetCurrentVariants()
        {
            var colors = _unitOfWork.Color.GetAll().Select(x => new DisplayColorDto { Id = x.Id, Name = x.Name });
            var sizes = _unitOfWork.Size.GetAll().Select(x => new DisplaySizeDto { Id = x.Id, Name = x.Name });

            var currentVariants = new DisplayVariantsDto { Colors = colors, Sizes = sizes };

            return new(currentVariants);
        }

        public ResponseDto<ProductSpecificationsDto?> GetProductSpecificationById(int productId)
        {
            var currentProduct =
                _unitOfWork
                .Product
                .GetAll()
                .Include(x => x.Stocks)
                .Include(x => x.Pictures)
                .Include(x => x.Category)
                .Include(x => x.Stocks)
                .ThenInclude(x => x.Color)
                .Include(x => x.Stocks)
                .ThenInclude(x => x.Size)
                .AsNoTracking()
                .FirstOrDefault(predicate: x => x.Id == productId);

            if (currentProduct is null)
                return new(StatusCodes.Status404NotFound, "O produto especificado não existe no sistema.");

            var product = _mapper.Map<ProductSpecificationsDto>(currentProduct);

            return new(product);
        }

        public ResponseDto<DisplayFiltersDto> GetAllFilters()
        {
            var allColors = _unitOfWork.Color.GetAll().Select(x => x.Name).ToList();
            var allBrands = _unitOfWork.Brand.GetAll().Select(x => x.Name).ToList();
            var allSizes = _unitOfWork.Size.GetAll().Select(x => x.Name).ToList();
            var allCatogories = _unitOfWork.Category.GetAll().Select(x => x.Name).ToList();

            allCatogories.Insert(0, "Todas");

            var filters = new DisplayFiltersDto
            {
                Brands = allBrands,
                Categories = allCatogories,
                Colors = allColors,
                Sizes = allSizes,
            };

            return new(filters);
        }

        public ResponseDto<IEnumerable<StockAvaliableByIdDto>> GetAvaliableStocks(IEnumerable<CountStockByItsIdDto> countStockByItsIdDtos)
        {
            var onlyStockIds = countStockByItsIdDtos.Select(x => x.StockId);

            var stocksDeterminedByItsCount = _unitOfWork
                .Stock
                .GetAll()
                .AsNoTracking()
                .Select(x => new StockAvaliableByIdDto
                {
                    StockId = x.Id,
                    IsAvaliable = countStockByItsIdDtos.Single(y => y.StockId == x.Id).StockCount <= x.Count,
                })
                .Where(x => onlyStockIds.Contains(x.StockId));

            return new(stocksDeterminedByItsCount);
        }

        public async Task<PaginatedResponseDto<IEnumerable<DisplayStubProduct>>> GetExistentPaginated(
            ExistentQueryDto existentQueryDto
            )
        {
            var paginatedProducts = await _unitOfWork.Product
                .GetAll()
                .Include(x => x.Pictures)
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .ProjectTo<DisplayStubProduct>(_mapper.ConfigurationProvider)
                .FilterByProductNameIfWasInformed(existentQueryDto.SearchQuery)
                .OrderByNameWithInformedDirection(existentQueryDto.Name)
                .OrderByPriceWithInformedDirection(existentQueryDto.Price)
                .OrderByCategoryWithInformedDirection(existentQueryDto.Category)
                .OrderByBrandWithInformedDirection(existentQueryDto.Brand)
                .GetCount(out int count)
                .Paginate(existentQueryDto.Page, existentQueryDto.PageSize)
                .ToListAsync();

            return new PaginatedResponseDto<IEnumerable<DisplayStubProduct>>(
                paginatedProducts,
                existentQueryDto.Page,
                existentQueryDto.PageSize,
                count);
        }

        public ResponseDto<bool> DisableProduct(int productId, bool isActive)
        {
            var currentProduct = _unitOfWork.Product.FindOne(x => x.Id == productId);

            if (currentProduct is null)
                return new(StatusCodes.Status404NotFound, "O produto a ser desativado não existe mais.");

            currentProduct.IsActive = isActive;

            _unitOfWork.Product.Update(currentProduct);
            _unitOfWork.Save();

            return new(StatusCodes.Status200OK, $"Produto {(isActive ? "ativado" : "desativado")} com sucesso!");
        }
    }
}
