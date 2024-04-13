using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace GbStoreApi.Data.Implementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext _dataContext;
        public ProductRepository(DataContext context) : base(context) {
            _dataContext = context;
        }
    }
}
