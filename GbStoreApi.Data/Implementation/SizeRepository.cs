using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class SizeRepository : GenericRepository<Size>, ISizeRepository
    {
        private readonly DataContext _context;
        public SizeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Size> GetByIdAndReturnsQueryable(int id)
        {
            return _context.Sizes.Where(x => x.Id == id).AsQueryable();
        }

        public Size? GetByName(string name)
        {
            return _context.Sizes.FirstOrDefault(x => x.Name == name);
        }
    }
}
