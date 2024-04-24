using GbStoreApi.Domain.Dto.Stocks;

namespace GbStoreApi.Application.Interfaces
{
    public interface IStockService
    {
        int CreateMultipleStock(CreateStockByProductIdDto createStockDto);
    }
}
