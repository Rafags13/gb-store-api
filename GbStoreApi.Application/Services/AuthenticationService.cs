using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Azure;
using System.Net;
using Microsoft.AspNetCore.Http;
using GbStoreApi.Application.Exceptions;

namespace GbStoreApi.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _context;
        private readonly IUserService _userService;
        public AuthenticationService(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IHttpContextAccessor context,
            IUserService userService
            ) 
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _context = context;
            _userService = userService;
        }

        public string SignIn(SignInDto signInDto)
        {
            var currentUser = _userService.GetByCredentials(signInDto);
            
            var userToken = _tokenService.CreateModelByUser(currentUser);

            var token = _tokenService.Generate(userToken);

            var refreshToken = _tokenService.GenerateRefresh();

            SetRefreshToken(refreshToken, currentUser.Id);

            return token;
        }

        private void SetRefreshToken(RefreshToken refreshToken, int userId) 
        {
            var cookieOptions = new CookieOptions
            { 
                HttpOnly = true,
                Expires = refreshToken.Expires,
            };

            _context.HttpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            var currentUser = _unitOfWork.User.FindOne(x => x.Id == userId);
            currentUser.RefreshToken = refreshToken.Token;
            currentUser.TokenCreated = refreshToken.Created;
            currentUser.TokenExpires = refreshToken.Expires;

            _unitOfWork.Save();
        }

        public string SignUp(SignUpDto signUpDto)
        {
            var newUser = new User
            {
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                Cpf = signUpDto.Cpf,
                Password = BCrypt.Net.BCrypt.HashPassword(signUpDto.Password),
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

        public string RefreshToken()
        {
            var refreshToken = _context?.HttpContext?.Request.Cookies["refreshToken"];
            var userId = _userService.GetCurrentInformations().Id;
            var currentUser = _unitOfWork.User.FindOne(x => x.Id == userId);

            if(!currentUser.RefreshToken.Equals(refreshToken))
            {
                throw new UnauthorizedAccessException("Você não possui acesso ao sistema.");
            }

            if(currentUser.TokenExpires < DateTime.Now)
            {
                throw new UnauthorizedAccessException("Seu tempo de sessão acabou. Entre novamente.");
            }

            var userToken = _tokenService.CreateModelByUser(currentUser);
            string newToken = _tokenService.Generate(userToken);

            var newRefreshToken = _tokenService.GenerateRefresh();
            SetRefreshToken(newRefreshToken, userId);

            return newToken;
        }
    }
}
