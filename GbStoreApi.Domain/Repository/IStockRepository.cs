using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Models;

namespace GbStoreApi.Domain.Repository
{
    public interface IStockRepository : IGenericRepository<ProductStock>
    {
    }
}
