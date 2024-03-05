using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.enums;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        public DisplayUserDto? GetCurrentInformations()
        {
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
    }
}
