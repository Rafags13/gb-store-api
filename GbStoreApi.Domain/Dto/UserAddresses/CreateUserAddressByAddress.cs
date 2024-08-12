using GbStoreApi.Domain.Dto.Address;

namespace GbStoreApi.Domain.Dto.UserAddresses
{
    public class CreateUserAddressByAddress
    {
        public CreateAddressDto Address { get; set; }
        public int UserId { get; set; }

        public CreateUserAddressByAddress(CreateAddressDto addressDto)
        {
            Address = addressDto;
        }
    }
}
