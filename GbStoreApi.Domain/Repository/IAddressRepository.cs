using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Models;

namespace GbStoreApi.Domain.Repository
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        public int? GetStoreAddressId();
        public int? GetAddressIdByZipCode(string zipCode);
    }
}
