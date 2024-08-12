using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Models;

namespace GbStoreApi.Domain.Repository
{
    public interface IColorRepository : IGenericRepository<Color>
    {
        Color? GetByName(string name);
        IQueryable<Color> GetByIdAndReturnsQueryable(int id);
    }
}
