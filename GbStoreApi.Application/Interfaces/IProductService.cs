using GbStoreApi.Domain.Dto;

namespace GbStoreApi.Application.Interfaces
{
    public interface IProductService
    {
        bool CreateProduct(CreateProductDto createProductDto);
        
    }
}
