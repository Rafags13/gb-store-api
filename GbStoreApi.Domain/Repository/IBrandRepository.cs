using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Models;

namespace GbStoreApi.Domain.Repository
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Brand? GetByName(string name);
        IQueryable<Brand> GetByIdAndReturnQueryable(int id);
    }
}
