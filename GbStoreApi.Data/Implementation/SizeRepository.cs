using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class SizeRepository : GenericRepository<Size>, ISizeRepository
    {
        public SizeRepository(DataContext context) : base(context)
        {
            
        }
    }
}
