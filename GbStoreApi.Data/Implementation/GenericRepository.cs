using GbStoreApi.Application.Interfaces;
using GbStoreApi.Data.Context;
using System.Linq.Expressions;

namespace GbStoreApi.Data.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _context;
        public GenericRepository(DataContext context)
        {
            _context = context;
        }
        public void Add(T Entity)
        {
            _context.Set<T>().Add(Entity);
        }

        public async Task AddAsync(T Entity)
        {
            await _context.Set<T>().AddAsync(Entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public T? FindOne(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }
        public T Update(T entity)
        {
            return _context.Set<T>().Update(entity).Entity;
        }

        public T Remove(T entity)
        {
            return _context.Set<T>().Remove(entity).Entity;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public bool Contains()
        {
            return _context.Set<T>().Any();
        }

        public bool Contains(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
        }
    }
}
