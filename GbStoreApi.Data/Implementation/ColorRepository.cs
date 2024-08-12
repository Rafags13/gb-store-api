using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        private readonly DataContext _context;
        public ColorRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public Color? GetByName(string name)
        {
            return _context.Set<Color>().FirstOrDefault(x => x.Name == name);
        }

        public IQueryable<Color> GetByIdAndReturnsQueryable(int id)
        {
            return _context.Set<Color>().AsQueryable().Where(x => x.Id == id);
        }

    }
}
