using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Address;
using GbStoreApi.Domain.Dto.Generic;

namespace GbStoreApi.Application.Services.Address
{
    public class AddressService : IAddressService
    {
        public ResponseDto<bool> Create(CreateAddressDto createAddressDto)
        {
            throw new NotImplementedException();
        }

        public ResponseDto<DisplayAddressDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public ResponseDto<DisplayAddressDto> GetAllByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public ResponseDto<DisplayAddressDto> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
