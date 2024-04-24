using GbStoreApi.Domain.Dto.Products;
using GbStoreApi.Domain.Dto.Product.Catalogs;
using GbStoreApi.Domain.Dto.Stocks;
using GbStoreApi.Domain.Dto.Stocks.Filters;

namespace GbStoreApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<bool> CreateProduct(CreateProductDto createProductDto);
        DisplayVariantsDto? GetCurrentVariants();
        IEnumerable<DisplayProductDto>? GetAll();
        IEnumerable<DisplayProductDto> GetByFilters(CatalogFilterDto filters);
        ProductSpecificationsDto? GetProductSpecificationById(int productId);
        DisplayFiltersDto GetAllFilters();
        IEnumerable<StockAvaliableByIdDto> GetAvaliableStocks(IEnumerable<CountStockByItsIdDto> countStockByItsIdDtos);
    }
}
