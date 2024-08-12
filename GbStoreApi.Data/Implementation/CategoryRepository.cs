using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GbStoreApi.Data.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DataContext _dataContext;
        public CategoryRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public Category? GetOneByName(string name)
        {
            return _dataContext.Categories.FirstOrDefault(x => x.Name == name);
        }

        public Category? GetOneById(int id)
        {
            return _dataContext.Categories.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Category> GetByIdAndReturnsQueryable(int id)
        {
            return _dataContext.Categories.Where(x => x.Id == id).AsQueryable();
        }
    }
}
