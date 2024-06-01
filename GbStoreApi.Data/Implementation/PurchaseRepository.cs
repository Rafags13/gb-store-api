using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models.Purchases;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class PurchaseRepository : GenericRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(DataContext context) : base(context) { }
    }
}
