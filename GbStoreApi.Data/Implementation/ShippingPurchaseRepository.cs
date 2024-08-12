using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models.Purchases;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class ShippingPurchaseRepository : GenericRepository<ShippingPurchase>, IShippingPurchaseRepository
    {
        public ShippingPurchaseRepository(DataContext context) : base(context)
        {
            
        }
    }
}
