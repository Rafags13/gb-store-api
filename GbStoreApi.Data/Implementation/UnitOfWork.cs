using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public UnitOfWork(DataContext context)
        {
            _context = context;
            User = new UserRepository(_context);
            Category = new CategoryRepository(_context);
            Product = new ProductRepository(_context);
            Stock = new StockRepository(_context);
            Color = new ColorRepository(_context);
            Size = new SizeRepository(_context);
            Brand = new BrandRepository(_context);
            Picture = new PictureRepository(_context);
            Address = new AddressRepository(_context);
            Purchase = new PurchaseRepository(_context);
        }

        public IUserRepository User { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IStockRepository Stock { get; private set; }
        public IColorRepository Color { get; private set; }
        public ISizeRepository Size { get; private set; }
        public IBrandRepository Brand { get; private set; }
        public IPictureRepository Picture { get; private set; }
        public IAddressRepository Address { get; private set; }
        public IPurchaseRepository Purchase { get; private set; }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
