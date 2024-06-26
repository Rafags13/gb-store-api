using GbStoreApi.Domain.Dto.Products;
using GbStoreApi.Domain.Dto.Product.Catalogs;
using GbStoreApi.Domain.Dto.Stocks;
using GbStoreApi.Domain.Dto.Stocks.Filters;
using GbStoreApi.Domain.Dto.Generic;

namespace GbStoreApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<ResponseDto<bool>> CreateProduct(CreateProductDto createProductDto);
        ResponseDto<DisplayVariantsDto?> GetCurrentVariants();
        ResponseDto<IEnumerable<DisplayProductDto>> GetAll();
        Task<PaginatedResponseDto<IEnumerable<DisplayProductDto>>> GetByFilters(CatalogFilterDto filters);
        Task<PaginatedResponseDto<IEnumerable<DisplayStubProduct>>> GetExistentPaginated(string? productName, int page = 0, int pageSize = 20);
        ResponseDto<ProductSpecificationsDto?> GetProductSpecificationById(int productId);
        ResponseDto<DisplayFiltersDto> GetAllFilters();
        ResponseDto<IEnumerable<StockAvaliableByIdDto>> GetAvaliableStocks(IEnumerable<CountStockByItsIdDto> countStockByItsIdDtos);
    }
}
