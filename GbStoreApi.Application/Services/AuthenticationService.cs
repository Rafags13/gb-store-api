using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.model;
using GbStoreApi.Domain.Repository;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;

namespace GbStoreApi.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        public AuthenticationService(
            IUnitOfWork unitOfWork,
            ITokenService tokenService
            ) 
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public string SignIn(SignInDto signInDto)
        {
            var currentUser = _unitOfWork.User.FindOne(x => x.Email == signInDto.Email && x.Password == signInDto.Password);

            if(currentUser == null)
            {
                throw new KeyNotFoundException("O usuário informado não existe no sistema.");
            }

            var token = _tokenService.Generate(
                new UserTokenDto { 
                    Id = currentUser.Id, 
                    Name = currentUser.Name, 
                    Email = currentUser.Email, 
                    TypeOfUser = currentUser.TypeOfUser
                });

            return token;

        }

        public string SignUp(SignUpDto signUpDto)
        {
            var newUser = new User
            {
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                Cpf = signUpDto.Cpf,
                Password = signUpDto.Password,
                BirthdayDate = signUpDto.BirthdayDate,
                TypeOfUser = (int) signUpDto.TypeOfUser,
            };

            _unitOfWork.User.Add(newUser);

            var responseId = _unitOfWork.Save();
            if (responseId <= 0)
            {
                throw new Exception("Não foi possível adicionar esse usuário ao sistema.");
            }

            return "Usuário adicionado com sucesso!";
        }
    }
}
