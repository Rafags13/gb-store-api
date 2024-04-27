using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly DataContext _context;
        public BrandRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Brand> GetByIdAndReturnQueryable(int id)
        {
            return _context.Brands.Where(x => x.Id == id).AsQueryable();
        }

        public Brand? GetByName(string name)
        {
            return _context.Brands.FirstOrDefault(x => x.Name == name);
        }
    }
}
