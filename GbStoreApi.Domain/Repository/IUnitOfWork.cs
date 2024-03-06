using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Domain.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        ICategoryRepository Category { get; }

        int Save();
    }
}
