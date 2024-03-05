using Microsoft.EntityFrameworkCore;
using GbStoreApi.Domain.model;

namespace GbStoreApi.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
