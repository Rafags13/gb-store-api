using AutoMapper;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Constants;
using GbStoreApi.Domain.Dto.Address;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.UserAddresses;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GbStoreApi.Application.Services.Addresses
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public AddressService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserService userService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }
        public ResponseDto<bool> Create(CreateAddressDto createAddressDto)
        {
            var currentUserId = _userService.GetLoggedUserId();
            var thisAddressAlsoBeenRegisterByUser = _unitOfWork.UserAddresses
                .GetAll()
                .Any(x => x.Address.ZipCode == createAddressDto.ZipCode && x.UserId == currentUserId);

            if (thisAddressAlsoBeenRegisterByUser)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "Esse endereço já foi cadastrado por você.");

            using var transaction = _unitOfWork.GetContext().BeginTransaction();

            UserAddress newUserAddress = new();

            if(AddressExistsInDatabaseByZipCode(createAddressDto.ZipCode))
            {
                var currentAddressId = _unitOfWork.Address.FindOne(x => x.ZipCode == createAddressDto.ZipCode)!.Id;
                newUserAddress = new UserAddress()
                {
                    AddressId = currentAddressId,
                    UserId = currentUserId,
                    Complement = createAddressDto.Complement,
                    Number = createAddressDto.Number
                };
            } else
            {
                newUserAddress = _mapper.Map<UserAddress>(new CreateUserAddressByAddress(createAddressDto), opt => opt.AfterMap((_, address) =>
                {
                    address.UserId = currentUserId;
                }));
            }

            _unitOfWork.UserAddresses.Add(newUserAddress);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "Não foi possível criar o novo endereço");

            transaction.Commit();

            return new ResponseDto<bool>(true, StatusCodes.Status201Created, "Endereço criado com sucesso!");
        }

        public ResponseDto<IEnumerable<DisplayAddressDto>> GetAll()
        {
            var addresses = _unitOfWork.Address.GetAll().Select(_mapper.Map<DisplayAddressDto>);

            return new ResponseDto<IEnumerable<DisplayAddressDto>>(addresses, StatusCodes.Status200OK);
        }

        public ResponseDto<IEnumerable<DisplayAddressDto>> GetAllByUserId()
        {
            var currentUserId = _userService.GetLoggedUserId();

            var addresses =
                _unitOfWork.UserAddresses
                    .GetAll()
                    .Include(x => x.Address)
                    .Select(_mapper.Map<DisplayAddressDto>)
                    .Where(x => x.UserId == currentUserId);
            
            return new ResponseDto<IEnumerable<DisplayAddressDto>>(addresses, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayAddressDto> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ResponseDto<bool> Update(UpdateAddressDto updateAddressDto)
        {
            var userAddress = GetUserAddressIdByZipCodeAndLoggedUser(updateAddressDto.ZipCode);

            if (userAddress is null)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "Você não pode editar um endereço que não é seu.");

            _mapper.Map(updateAddressDto, userAddress);
            _unitOfWork.UserAddresses.Update(userAddress);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "Não foi possível editar o endereço. Tente Novamente.");

            return new ResponseDto<bool>(true, StatusCodes.Status200OK);
        }

        public ResponseDto<bool> Remove(string zipCode)
        {
            var currentAddressToRemove = GetUserAddressIdByZipCodeAndLoggedUser(zipCode);

            if (currentAddressToRemove is null)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "Você não pode editar um endereço que não é seu.");

            _unitOfWork.UserAddresses.Remove(currentAddressToRemove);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "Não foi possível remover o endereço. Tente Novamente.");

            return new ResponseDto<bool>(StatusCodes.Status200OK);
        }

        public ResponseDto<int> GetAddressIdByZipCode(string zipcode)
        {
            var currentUserId = _userService.GetLoggedUserId();

            var currentAddressId =
                _unitOfWork
                .Address
                .FindOne(x => x.ZipCode == zipcode);

            if (currentAddressId is null)
                return new ResponseDto<int>(StatusCodes.Status404NotFound, "Não foi possível encontrar o endereço.");

            return new ResponseDto<int>(currentAddressId.Id, StatusCodes.Status200OK);
        }

        public ResponseDto<int> GetAddressIdFromStorePickup()
        {
            var addressId = _unitOfWork.UserAddresses
                .FindOne(x => x.UserId == AdminProfileConstants.USER_ID)?.AddressId;

            if (addressId is null)
                return new ResponseDto<int>(StatusCodes.Status400BadRequest, "Não foi possível buscar o endereço da loja.");

            return new ResponseDto<int>(addressId.GetValueOrDefault(), StatusCodes.Status200OK);
        }

        private bool AddressExistsInDatabaseByZipCode(string zipCode)
        {
            return _unitOfWork.Address.GetAll().Any(x => x.ZipCode.Equals(zipCode));
        }

        private UserAddress? GetUserAddressIdByZipCodeAndLoggedUser(string zipCode)
        {
            var currentLoggedUser = _userService.GetLoggedUserId();
            var userAddress = _unitOfWork.UserAddresses
                .FindOne(x => x.UserId == currentLoggedUser
                    && x.Address.ZipCode == zipCode);

            return userAddress;
        }
    }
}
