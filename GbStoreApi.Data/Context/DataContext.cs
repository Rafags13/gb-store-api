using Microsoft.EntityFrameworkCore;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.enums;

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
        public DbSet<UserAddress> UserAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { 
                    Id = 1,
                    Name = "Administrador",
                    Cpf = "00000000000",
                    Email = "admin@gmail.com", // this can be also changed or updated in future 
                    Password = "$2a$11$t6XFpMSG74tuVVOCidtmQeXdqyteWbTBIqe29uC98goiLtzqiZdzC",
                    BirthdayDate =  DateTime.Now,
                    TypeOfUser = UserType.Administrator,
                });
        }
    }
}
