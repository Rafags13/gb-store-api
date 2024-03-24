using GbStoreApi.Domain.Dto;

namespace GbStoreApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<bool> CreateProduct(CreateProductDto createProductDto);
    }
}
