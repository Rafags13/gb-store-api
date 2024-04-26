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
            return _dataContext.Set<Category>().FirstOrDefault(x => x.Name == name);
        }

        public Category? GetOneById(int id)
        {
            return _dataContext.Set<Category>().FirstOrDefault(x => x.Id == id);
        }
    }
}
