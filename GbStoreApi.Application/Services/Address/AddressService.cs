using AutoMapper;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Address;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;

namespace GbStoreApi.Application.Services.Address
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddressService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public ResponseDto<bool> Create(CreateAddressDto createAddressDto)
        {
            throw new NotImplementedException();
        }

        public ResponseDto<IEnumerable<DisplayAddressDto>> GetAll()
        {
            var addresses = _unitOfWork.Address.GetAll().Select(_mapper.Map<DisplayAddressDto>);

            return new ResponseDto<IEnumerable<DisplayAddressDto>>(addresses, StatusCodes.Status200OK);
        }

        public ResponseDto<IEnumerable<DisplayAddressDto>> GetAllByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public ResponseDto<DisplayAddressDto> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
