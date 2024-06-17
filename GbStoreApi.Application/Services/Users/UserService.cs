using AutoMapper;
using Azure;
using GbStoreApi.Application.Exceptions;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Authentications;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Users;
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

            return new ResponseDto<IEnumerable<DisplayUserDto>>(users);
        }

        public ResponseDto<DisplayUserDto> GetById(int id)
        {

            var selectedUser =
                _unitOfWork.User.GetById(id);

            if (selectedUser is null)
                return new ResponseDto<DisplayUserDto>(StatusCodes.Status404NotFound, "Nenhum usuário encontrado.");
            
            var userMapped = _mapper.Map<DisplayUserDto>(selectedUser);


            return new ResponseDto<DisplayUserDto>(userMapped);
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
            
            return new ResponseDto<User>(currentUser);
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

            return new ResponseDto<DisplayUserDto>(displayUser);
        }

        public ResponseDto<string?> GetUserRole()
        {
            try
            {
                var currentUserId = GetLoggedUserId();

                var currentUser = _unitOfWork.User.FindOne(x => x.Id == currentUserId);

                if (currentUser is null)
                    return new ResponseDto<string?>(StatusCodes.Status404NotFound, "Não foi possível encontrar o usuário.");

                return new ResponseDto<string?>(currentUser.TypeOfUser.ToString());
            }
            catch (UserNotValidException)
            {
                return new ResponseDto<string?>(null);
            }
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

            return new ResponseDto<bool>(true);
        }

        public ResponseDto<bool> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var currentUserResponse = GetCurrentInformations();

            var currentUser = _unitOfWork.User.FindOne(x => x.Id == currentUserResponse.Value.Id);
            if (currentUser is null)
                return new ResponseDto<bool>(StatusCodes.Status404NotFound, "Não foi possível encontrar o usuário");

            var isSamePassword = BCrypt.Net.BCrypt.Verify(updatePasswordDto.OldPassword, currentUser.Password);
            if (!isSamePassword)
                return new ResponseDto<bool>(StatusCodes.Status400BadRequest, "A senha informada não é equivalente à antiga.");

            currentUser.Password = BCrypt.Net.BCrypt.HashPassword(updatePasswordDto.NewPassword);
            _unitOfWork.User.Update(currentUser);

            if (_unitOfWork.Save() == 0)
                return new ResponseDto<bool>(StatusCodes.Status422UnprocessableEntity, "Não foi possível atualizar a senha.");

            return new ResponseDto<bool>(StatusCodes.Status200OK);
        }

        public int GetLoggedUserId()
        {
            var currentUserIdInClaims = _context.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(currentUserIdInClaims, out int currentLoggedUserId))
                throw new UserNotValidException("Não foi possível recuperar o usuário logado");

            return currentLoggedUserId;
        }
    }
}
