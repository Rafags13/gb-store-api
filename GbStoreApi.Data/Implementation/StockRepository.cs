using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class StockRepository : GenericRepository<ProductStock>, IStockRepository
    {
        public StockRepository(DataContext context) : base(context)
        {
            
        }
    }
}
