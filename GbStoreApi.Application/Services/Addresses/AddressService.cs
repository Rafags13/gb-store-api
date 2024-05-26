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
            var currentLoggedUser = _userService.GetCurrentInformations();
            if (currentLoggedUser.StatusCode != StatusCodes.Status200OK || currentLoggedUser.Value is null)
                return new ResponseDto<bool>(StatusCodes.Status401Unauthorized, "Usuário não autorizado.");

            var currentUserId = currentLoggedUser.Value.Id;
            using var transaction = _unitOfWork.GetContext().BeginTransaction();

            if(AddressExistsInDatabaseByZipCode(createAddressDto.ZipCode))
            {
                var currentAddressId = _unitOfWork.Address.FindOne(x => x.ZipCode == createAddressDto.ZipCode).Id;
                var addressToUser = new UserAddress(currentUserId, currentAddressId);

                _unitOfWork.UserAddresses.Add(addressToUser);
            } else
            {
                var addressWithUser = _mapper.Map<UserAddress>(new CreateUserAddressByAddress(createAddressDto), opt => opt.AfterMap((_, address) =>
                {
                    address.UserId = currentUserId;
                }));
                _unitOfWork.UserAddresses.Add(addressWithUser);
            }


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
            var currentLoggedUser = _userService.GetCurrentInformations();
            if (currentLoggedUser.StatusCode != StatusCodes.Status200OK || currentLoggedUser.Value is null)
                return new ResponseDto<IEnumerable<DisplayAddressDto>>(StatusCodes.Status401Unauthorized, "Usuário não autorizado.");

            var currentUserId = currentLoggedUser.Value.Id;

            var userExists = _unitOfWork.User.Contains(x => x.Id == currentLoggedUser.Value.Id);
            if (!userExists)
                return new ResponseDto<IEnumerable<DisplayAddressDto>>(StatusCodes.Status404NotFound, "O usuário informado não existe.");

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

        private ResponseDto<Address> GetCurrentAddressByZipCode(string zipCode)
        {
            var currentLoggedUser = _userService.GetCurrentInformations();
            if (currentLoggedUser.StatusCode != StatusCodes.Status200OK || currentLoggedUser.Value is null)
                return new ResponseDto<Address>(StatusCodes.Status401Unauthorized, "Usuário não autorizado.");

            var currentUserId = currentLoggedUser.Value.Id;

            var currentAddressFromUser = _unitOfWork.Address.Find(x => x.ZipCode == zipCode).SingleOrDefault();
            if (currentAddressFromUser is null)
                return new ResponseDto<Address>(StatusCodes.Status400BadRequest, "Você não pode editar um endereço do qual não é seu!");

            return new ResponseDto<Address>(currentAddressFromUser, StatusCodes.Status200OK);
        }
        public ResponseDto<bool> Update(UpdateAddressDto updateAddressDto)
        {
            var response = GetCurrentAddressByZipCode(updateAddressDto.ZipCode);

            if (response.Value is null || response.StatusCode != StatusCodes.Status200OK)
                return new ResponseDto<bool>(response.StatusCode, response.Message!);

            _mapper.Map(updateAddressDto, response.Value);
            _unitOfWork.Address.Update(response.Value);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "Não foi possível editar o endereço. Tente Novamente.");

            return new ResponseDto<bool>(true, StatusCodes.Status200OK);
        }

        public ResponseDto<bool> Remove(string zipCode)
        {
            var response = GetCurrentAddressByZipCode(zipCode);

            if (response.Value is null || response.StatusCode != StatusCodes.Status200OK)
                return new ResponseDto<bool>(response.StatusCode, response.Message!);

            _unitOfWork.Address.Remove(response.Value);
            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "Não foi possível remover o endereço. Tente Novamente.");

            return new ResponseDto<bool>(StatusCodes.Status200OK);
        }

        public ResponseDto<int> GetAddressIdByZipCode(string zipcode)
        {
            var currentLoggedUser = _userService.GetCurrentInformations();
            if (currentLoggedUser.StatusCode != StatusCodes.Status200OK || currentLoggedUser.Value is null)
                return new ResponseDto<int>(StatusCodes.Status401Unauthorized, "Usuário não autorizado.");

            var currentUserId = currentLoggedUser.Value.Id;

            var currentAddressId =
                _unitOfWork
                .Address
                .GetAll()
                .FirstOrDefault(predicate: x => x.ZipCode == zipcode);

            if (currentAddressId is null)
                return new ResponseDto<int>(StatusCodes.Status404NotFound, "Não foi possível encontrar o endereço.");

            return new ResponseDto<int>(currentAddressId.Id, StatusCodes.Status200OK);
        }

        public ResponseDto<int> GetAddressIdFromStorePickup()
        {
            var addressId = _unitOfWork.UserAddresses.GetAll().FirstOrDefault(predicate: x => x.UserId == AdminProfileConstants.USER_ID)?.AddressId;
            if (addressId is null)
                return new ResponseDto<int>(StatusCodes.Status400BadRequest, "Não foi possível buscar o endereço da loja.");

            return new ResponseDto<int>(addressId.GetValueOrDefault(), StatusCodes.Status200OK);
        }

        private bool AddressExistsInDatabaseByZipCode(string zipCode)
        {
            return _unitOfWork.Address.GetAll().Any(x => x.ZipCode.Equals(zipCode));
        }
    }
}
