using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        public ColorRepository(DataContext context) : base(context)
        {
            
        }
    }
}
