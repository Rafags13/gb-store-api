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
