﻿using AutoMapper;
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
        public async Task<bool> CreateProduct(CreateProductDto createProductDto)
        {
            var newProduct = _mapper.Map<Product>(createProductDto);

            _unitOfWork.Product.Add(newProduct);

            if (_unitOfWork.Save() < 1)
            {
                throw new CantCreateProductException("Não foi possível criar o produto informado.");
            }

            var currentProductId = _unitOfWork.Product.FindOne(x => x.Name == createProductDto.Name).Id;

            var newStockToProduct = new CreateStockByProductIdDto { ProductId = currentProductId, Variants = createProductDto.Stock };

            var createStockSuccess = _stockService.CreateMultipleStock(newStockToProduct) > 0;
            if (!createStockSuccess) throw new CantCreateProductException("Não foi possível criar os estoques. Fale com o administrador do sistema.");

            var picturesName = await _fileService.CreateMultipleFiles(createProductDto.Photos);

            var picturesToCreate = picturesName.Select(name => name);

            if (!picturesToCreate.Any()) return false;

            var picturesWithProductId = new PicturesGroupedByProductDto
            {
                Pictures = picturesToCreate,
                ProductId = currentProductId
            };

            _pictureService.CreateMultiplePictures(picturesWithProductId); // refactor

            return true;
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
                .Paginate()
                .Select(_mapper.Map<DisplayProductDto>) ?? Enumerable.Empty<DisplayProductDto>();

            if (!productsReference.Any())
                return new ResponseDto<IEnumerable<DisplayProductDto>>(productsReference, StatusCodes.Status404NotFound, "Nenhum produto foi encontrado.");

            return new ResponseDto<IEnumerable<DisplayProductDto>>(productsReference, StatusCodes.Status200OK);
        }

        public PaginatedResponseDto<IEnumerable<DisplayProductDto>> GetByFilters(CatalogFilterDto filters)
        {
            var productsFiltered = _unitOfWork.Product
                .GetAll()
                .WithPicturesFromStock()
                .WithSizes()
                .WithColors()
                .FilterByCategoryIfWasInformed(filters.Category)
                .FilterByColorsIfWereInformed(filters.Colors)
                .FilterBySizesIfWereInformed(filters.Sizes)
                .OrderBy(direction: filters.Direction ?? "ascending", fieldName: filters.OrderBy ?? "");
            // refactor this query

            // Implement here a function that Recieves the name of field and orderBy it, with
            // no conditional compensation for any existent field

            var dontExistsProductsInThisPage = !productsFiltered.Paginate(page: filters.Page, pageSize: filters.PageSize).Any();

            var INITIAL_PAGE = 0;

            if (dontExistsProductsInThisPage)
                filters.Page = INITIAL_PAGE;

            var totalFiltered = productsFiltered.Count();

            var paginatedProducts = productsFiltered
                .Paginate(page: filters.Page, pageSize: filters.PageSize)
                .AsNoTracking()
                .Select(_mapper.Map<DisplayProductDto>).ToList();

            if (!paginatedProducts.Any() || paginatedProducts.Count() == 0)
                return new PaginatedResponseDto<IEnumerable<DisplayProductDto>>(
                    StatusCodes.Status404NotFound, 
                    "Não foi encontrado nenhum produto.",
                    filters.Page,
                    filters.PageSize
                    );

            return new PaginatedResponseDto<IEnumerable<DisplayProductDto>>(
                paginatedProducts,
                StatusCodes.Status200OK,
                filters.Page,
                filters.PageSize,
                totalFiltered
                );
        }

        public DisplayVariantsDto? GetCurrentVariants()
        {
            var colors = _unitOfWork.Color.GetAll().Select(x => new DisplayColorDto { Id = x.Id, Name = x.Name });
            var sizes = _unitOfWork.Size.GetAll().Select(x => new DisplaySizeDto { Id = x.Id, Name = x.Name });

            var currentVariants = new DisplayVariantsDto { Colors = colors, Sizes = sizes };

            return currentVariants;
        }

        public ProductSpecificationsDto? GetProductSpecificationById(int productId)
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

            if (currentProduct is null) throw new ProductNotFoundException("O produto especificado não existe no sistema.");
            return _mapper.Map<ProductSpecificationsDto>(currentProduct);
        }

        public DisplayFiltersDto GetAllFilters()
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

            return filters;
        }

        public IEnumerable<StockAvaliableByIdDto> GetAvaliableStocks(IEnumerable<CountStockByItsIdDto> countStockByItsIdDtos)
        {
            var onlyStockIds = countStockByItsIdDtos.Select(x => x.StockId);
            var stocksDeterminedByItsCount = _unitOfWork
                .Stock
                .GetAll()
                .AsEnumerable()
                .IntersectBy(onlyStockIds, x => x.Id)
                .Select(x => new StockAvaliableByIdDto
                {
                    StockId = x.Id,
                    IsAvaliable = countStockByItsIdDtos.Single(y => y.StockId == x.Id).StockCount <= x.Count,
                });

            return stocksDeterminedByItsCount;
        }
    }
}
