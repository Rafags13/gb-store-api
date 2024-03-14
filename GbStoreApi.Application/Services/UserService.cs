using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.enums;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GbStoreApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;
        public UserService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor context
            ) 
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public IEnumerable<DisplayUserDto> GetAll()
        {
            var users = 
                _unitOfWork.User.GetAll()
                    .Select(x => new DisplayUserDto 
                    {
                        Cpf = x.Cpf,
                        Email = x.Email,
                        Id = x.Id,
                        Name = x.Name,
                        TypeOfUser = (UserType) x.TypeOfUser,
                    });

            return users;
        }

        public DisplayUserDto? GetById(int id)
        {

            var selectedUser =
                _unitOfWork.User.GetById(id);

            if (selectedUser is null)
            {
                throw new Exception("Usuário não encontrado");
            }

            var userCorrectTyped = new DisplayUserDto
            {
                Id = selectedUser.Id,
                Name = selectedUser.Name,
                Cpf = selectedUser.Cpf,
                Email = selectedUser.Email,
                TypeOfUser = (UserType)selectedUser.TypeOfUser
            };


            return userCorrectTyped;
        }

        public User? GetByCredentials(SignInDto signInDto)
        {
            var encryptPassword = BCrypt.Net.BCrypt.HashPassword(signInDto.Password);

            var currentUser = _unitOfWork.User.FindOne(x =>
                x.Email == signInDto.Email
            );

            if(currentUser is null || !BCrypt.Net.BCrypt.Verify(signInDto.Password, currentUser.Password))
            {
                return null;
            }

            return currentUser;
        }

        public DisplayUserDto? GetCurrentInformations()
        {
            if (!_context.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("Você precisa se autenticar para obter mais informações");
            }

            var currentUserEmail = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var currentUser = _unitOfWork.User.FindOne(x => x.Email == currentUserEmail);
            if (currentUser is null)
            {
                throw new Exception("Não foi possível encontrar o usuário requisitado.");
            }

            var displayUser = new DisplayUserDto { 
                Id = currentUser.Id,
                Cpf = currentUser.Cpf,
                Email = currentUser.Email,
                Name = currentUser.Name,
                TypeOfUser = (UserType)currentUser.TypeOfUser
            };

            return displayUser;
        }

        public UserType? GetUserRole()
        {
            var currentUserEmail = _context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            if (currentUserEmail is null) return null;

            var user = _unitOfWork.User.FindOne(x => x.Email == currentUserEmail);

            if (user is null) return null;

            return (UserType) user.TypeOfUser;
        }
    }
}
