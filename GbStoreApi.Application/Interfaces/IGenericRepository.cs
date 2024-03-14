using System.Linq.Expressions;

namespace GbStoreApi.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T Entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T Entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
