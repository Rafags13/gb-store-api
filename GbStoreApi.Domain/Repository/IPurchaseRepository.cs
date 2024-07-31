using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Models.Purchases;

namespace GbStoreApi.Domain.Repository
{
    public interface IPurchaseRepository : IGenericRepository<Purchase>
    {
        int CountByMonthIndex(int monthIndex);
        decimal SumByMonthIndex(int monthIndex);
    }
}
