using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Models;

namespace GbStoreApi.Domain.Repository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Category? GetOneByName(string name);
        Category? GetOneById (int id);
    }
}
