using AutoMapper;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Address;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;

namespace GbStoreApi.Application.Services.Addresses
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
            var address = _mapper.Map<Address>(createAddressDto);

            _unitOfWork.Address.Add(address);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "Não foi possível criar o novo endereço");

            return new ResponseDto<bool>(true, StatusCodes.Status201Created, "Endereço criado com sucesso!");
        }

        public ResponseDto<IEnumerable<DisplayAddressDto>> GetAll()
        {
            var addresses = _unitOfWork.Address.GetAll().Select(_mapper.Map<DisplayAddressDto>);

            return new ResponseDto<IEnumerable<DisplayAddressDto>>(addresses, StatusCodes.Status200OK);
        }

        public ResponseDto<IEnumerable<DisplayAddressDto>> GetAllByUserId(int userId)
        {
            var userExists = _unitOfWork.User.Contains(x => x.Id == userId);
            if (!userExists)
                return new ResponseDto<IEnumerable<DisplayAddressDto>>(StatusCodes.Status404NotFound, "O usuário informado não existe.");

            var addressesByUserId = _unitOfWork.Address.GetAll().Where(x => x.UserId == userId).Select(_mapper.Map<DisplayAddressDto>);

            return new ResponseDto<IEnumerable<DisplayAddressDto>>(addressesByUserId, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayAddressDto> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
