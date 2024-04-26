using AutoMapper;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Authentications;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Users;
using GbStoreApi.Domain.enums;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GbStoreApi.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;
        private readonly IMapper _mapper;
        public UserService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor context,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
        }
        public ResponseDto<IEnumerable<DisplayUserDto>> GetAll()
        {
            var users = _unitOfWork.User.GetAll().Select(user => _mapper.Map<DisplayUserDto>(user));
            if (!users.Any() || users is null)
                return new ResponseDto<IEnumerable<DisplayUserDto>>(
                    statusCode: StatusCodes.Status404NotFound, 
                    "Não existe nenhum usuário cadastrado no sistema.");

            return new ResponseDto<IEnumerable<DisplayUserDto>>(users, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayUserDto> GetById(int id)
        {

            var selectedUser =
                _unitOfWork.User.GetById(id);

            if (selectedUser is null)
                return new ResponseDto<DisplayUserDto>(StatusCodes.Status404NotFound, "Nenhum usuário encontrado.");
            
            var userMapped = _mapper.Map<DisplayUserDto>(selectedUser);


            return new ResponseDto<DisplayUserDto>(userMapped, StatusCodes.Status200OK);
        }

        public User? GetByCredentials(SignInDto signInDto)
        {

            var currentUser = _unitOfWork.User.FindOne(x =>
                x.Email == signInDto.Email
            );

            var passwordsMatch = BCrypt.Net.BCrypt.Verify(signInDto.Password, currentUser.Password);

            if (currentUser is null || !BCrypt.Net.BCrypt.Verify(signInDto.Password, currentUser.Password))
            {
                return null;
            }

            return currentUser;
        }

        public ResponseDto<DisplayUserDto> GetCurrentInformations()
        {
            var currentUserEmail = _context.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(currentUserEmail))
                return new ResponseDto<DisplayUserDto>(StatusCodes.Status401Unauthorized, "Usuário inválido");

            var currentUser = _unitOfWork.User.FindOne(x => x.Email == currentUserEmail);
            if (currentUser is null)
                return new ResponseDto<DisplayUserDto>(StatusCodes.Status404NotFound, "Não foi encontrado nenhum usuário com esse e-mail.");

            var displayUser = _mapper.Map<DisplayUserDto>(currentUser);

            return new ResponseDto<DisplayUserDto>(displayUser, StatusCodes.Status200OK);
        }

        public ResponseDto<UserType> GetUserRole()
        {
            var response = GetCurrentInformations();

            if (response.StatusCode != StatusCodes.Status200OK || response.Value is null)
                return new ResponseDto<UserType>(response.StatusCode, response.Message!);

            return new ResponseDto<UserType>(response.Value.TypeOfUser, StatusCodes.Status200OK);
        }
    }
}
