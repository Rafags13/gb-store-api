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
        IProductRepository Product { get; }
        IStockRepository Stock { get; }
        ISizeRepository Size { get; }
        IColorRepository Color { get; }
        IBrandRepository Brand { get; }

        int Save();
    }
}
