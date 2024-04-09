using GbStoreApi.Application.Interfaces;
using GbStoreApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

        public T FindOne(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        //public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
        //{
        //    return source.Include(navigationPropertyPath);
        //}

        //public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) where TEntity : class
        //{
        //    return source.ThenInclude(navigationPropertyPath);
        //}
    }
}
