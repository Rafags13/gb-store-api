using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class UserAddressesRepository : GenericRepository<UserAddress>, IUserAddressesRepository
    {
        private readonly DataContext _dataContext;
        public UserAddressesRepository(DataContext context):base(context)
        {
            _dataContext = context;
        }

        public Guid? GetUserAddressIdByUserAndZipCode(string zipCode, int userId)
        {
            return _dataContext.UserAddresses.FirstOrDefault(x => x.Address.ZipCode == zipCode && x.UserId == userId)?.Id;
        }
    }
}
