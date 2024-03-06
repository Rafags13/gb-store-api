using GbStoreApi.Application.Interfaces;
using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Data.Implementation
{
    public class StockRepository : GenericRepository<ProductStock>, IStockRepository
    {
        public StockRepository(DataContext context) : base(context)
        {
            
        }
    }
}
