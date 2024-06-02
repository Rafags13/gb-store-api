using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models.Purchases;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class StorePickupPurchaseRepository : GenericRepository<StorePickupPurchase>, IStorePickupPurchaseRepository
    {
        public StorePickupPurchaseRepository(DataContext context) : base(context)
        {
            
        }
    }
}
