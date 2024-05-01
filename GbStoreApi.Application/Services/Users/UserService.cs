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
            var users = _unitOfWork.User.GetAll().Select(user => _mapper.Map<DisplayUserDto>(user)) ??
                Enumerable.Empty<DisplayUserDto>().AsQueryable();

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

        public ResponseDto<User> GetByCredentials(SignInDto signInDto)
        {
            var currentUser = _unitOfWork.User.FindOne(x =>
                x.Email == signInDto.Email
            );

            if (currentUser is null)
                return new ResponseDto<User>(StatusCodes.Status404NotFound, "Não existe nenhum usuário com esse e-mail. Tente Novamente.");

            var passwordsMatch = BCrypt.Net.BCrypt.Verify(signInDto.Password, currentUser.Password);

            if (!passwordsMatch)
                return new ResponseDto<User>(StatusCodes.Status404NotFound, "A senha informada está incorreta. Tente Novamente.");
            
            return new ResponseDto<User>(currentUser, StatusCodes.Status200OK);
        }

        public ResponseDto<DisplayUserDto> GetCurrentInformations()
        {
            var currentUserId = _context.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
                return new ResponseDto<DisplayUserDto>(StatusCodes.Status401Unauthorized, "Usuário inválido");

            var currentUser = _unitOfWork.User.FindOne(x => x.Id == int.Parse(currentUserId));
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

        public ResponseDto<bool> Update(UpdateUserDto updateUserDto)
        {
            var currentUser = _unitOfWork.User.GetAll().FirstOrDefault(x => x.Cpf == updateUserDto.Cpf);

            if (currentUser is null)
                return new ResponseDto<bool>(StatusCodes.Status404NotFound, "Não existe nenhum usuário com esse cpf.");

            _mapper.Map(updateUserDto, currentUser);

            _unitOfWork.User.Update(currentUser);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "Não foi possível salvar as alterações do usuário.");

            return new ResponseDto<bool>(true, StatusCodes.Status200OK);
        }
    }
}
