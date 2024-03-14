using GbStoreApi.Domain.Dto;

namespace GbStoreApi.Application.Interfaces
{
    public interface IStockService
    {
        int CreateMultipleStock(CreateStockWithIdDto createStockDto);
    }
}
