using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Models;

namespace GbStoreApi.Domain.Repository
{
    public interface ISizeRepository : IGenericRepository<Size>
    {
        Size? GetByName(string name);
        IQueryable<Size> GetByIdAndReturnsQueryable(int id);
    }
}
