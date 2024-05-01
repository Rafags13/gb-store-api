using Microsoft.EntityFrameworkCore;
using GbStoreApi.Domain.Models;

namespace GbStoreApi.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
