using GbStoreApi.Domain.Dto.Address;
using GbStoreApi.Domain.Dto.Generic;

namespace GbStoreApi.Application.Interfaces
{
    public interface IAddressService
    {
        ResponseDto<bool> Create(CreateAddressDto createAddressDto);
        ResponseDto<IEnumerable<DisplayAddressDto>> GetAll();
        ResponseDto<IEnumerable<DisplayAddressDto>> GetAllByUserId();
        ResponseDto<DisplayAddressDto> GetById(int id);
    }
}
