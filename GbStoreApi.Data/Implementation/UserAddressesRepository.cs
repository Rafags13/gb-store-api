using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class UserAddressesRepository : GenericRepository<UserAddress>, IUserAddressesRepository
    {
        public UserAddressesRepository(DataContext context):base(context)
        {
            
        }
    }
}
