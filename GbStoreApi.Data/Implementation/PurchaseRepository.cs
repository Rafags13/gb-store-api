using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models.Purchases;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class PurchaseRepository : GenericRepository<Purchase>, IPurchaseRepository
    {
        private readonly DataContext _dataContext;
        public PurchaseRepository(DataContext context) : base(context) {
            _dataContext = context;
        }

        public int CountByMonthIndex(int monthIndex)
        {
            return _dataContext.Purchases.Count(x => x.OrderDate.Month == monthIndex);
        }

        public decimal SumByMonthIndex(int monthIndex)
        {
            return _dataContext.Purchases.Where(x => x.OrderDate.Month == monthIndex).Sum(x => x.FinalPrice);
        }
    }
}
