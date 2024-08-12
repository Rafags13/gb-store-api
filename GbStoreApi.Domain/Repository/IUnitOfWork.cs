using Microsoft.EntityFrameworkCore.Infrastructure;

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
        IPictureRepository Picture { get; }
        IAddressRepository Address { get; }
        IPurchaseRepository Purchase { get; }
        IShippingPurchaseRepository ShippingPurchase { get; }
        IStorePickupPurchaseRepository StorePickupPurchase { get; }
        IUserAddressesRepository UserAddresses { get; }
        DatabaseFacade GetContext();
        int Save();
    }
}
