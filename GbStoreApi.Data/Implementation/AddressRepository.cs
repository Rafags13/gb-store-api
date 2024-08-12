using GbStoreApi.Data.Context;
using GbStoreApi.Domain.Constants;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;

namespace GbStoreApi.Data.Implementation
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private readonly DataContext _dataContext;
        public AddressRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }

        public int? GetStoreAddressId() =>
            _dataContext.UserAddresses.FirstOrDefault(x => x.UserId == AdminProfileConstants.USER_ID)?.AddressId;

        public int? GetAddressIdByZipCode(string zipCode) =>
            _dataContext.Addresses.FirstOrDefault(x => x.ZipCode == zipCode)?.Id;
    }
}
