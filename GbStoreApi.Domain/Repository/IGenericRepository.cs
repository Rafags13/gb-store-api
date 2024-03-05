using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T FindOne(Expression<Func<T, bool>> predicate);
        void Add(T Entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T Entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
