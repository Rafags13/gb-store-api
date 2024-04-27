using System.Linq.Expressions;

namespace GbStoreApi.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        IQueryable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T FindOne(Expression<Func<T, bool>> predicate);
        void Add(T Entity);
        void AddRange(IEnumerable<T> entities);
        T Remove(T Entity);
        void RemoveRange(IEnumerable<T> entities);
        bool Contains();
        bool Contains(Expression<Func<T, bool>> predicate);
        T Update(T entity);
    }
}
