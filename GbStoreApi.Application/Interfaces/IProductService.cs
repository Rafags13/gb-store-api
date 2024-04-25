using GbStoreApi.Domain.Dto.Products;
using GbStoreApi.Domain.Dto.Product.Catalogs;
using GbStoreApi.Domain.Dto.Stocks;
using GbStoreApi.Domain.Dto.Stocks.Filters;
using GbStoreApi.Domain.Dto.Generic;

namespace GbStoreApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<bool> CreateProduct(CreateProductDto createProductDto);
        DisplayVariantsDto? GetCurrentVariants();
        ResponseDto<IEnumerable<DisplayProductDto>>? GetAll();
        PaginatedResponseDto<IEnumerable<DisplayProductDto>> GetByFilters(CatalogFilterDto filters);
        ProductSpecificationsDto? GetProductSpecificationById(int productId);
        DisplayFiltersDto GetAllFilters();
        IEnumerable<StockAvaliableByIdDto> GetAvaliableStocks(IEnumerable<CountStockByItsIdDto> countStockByItsIdDtos);
    }
}
