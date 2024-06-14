using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Models;
using GbStoreApi.Domain.Repository;
using Microsoft.AspNetCore.Http;
using GbStoreApi.Domain.Dto.Authentications;
using AutoMapper;
using GbStoreApi.Domain.Dto.Users;
using GbStoreApi.Domain.Dto.Generic;

namespace GbStoreApi.Application.Services.Authentications
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AuthenticationService(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IHttpContextAccessor context,
            IUserService userService,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }

        public ResponseDto<string> SignIn(SignInDto signInDto)
        {
            var currentUser = _userService.GetByCredentials(signInDto);

            if (currentUser.StatusCode != StatusCodes.Status200OK || currentUser.Value is null)
                return new ResponseDto<string>(currentUser.StatusCode, currentUser.Message!);

            var userToken = _mapper.Map<UserTokenDto>(currentUser.Value);

            var token = _tokenService.Generate(userToken);

            var refreshToken = _tokenService.GenerateRefresh();

            SetRefreshToken(refreshToken, currentUser.Value.Id);

            return new ResponseDto<string>(token, StatusCodes.Status200OK);
        }

        private void SetRefreshToken(RefreshToken refreshToken, int userId)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = refreshToken.TokenExpires,
                SameSite = SameSiteMode.Strict,
                HttpOnly = true
            }; 

            _context.HttpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            var currentUser = _unitOfWork.User.FindOne(x => x.Id == userId);
            _mapper.Map(refreshToken, currentUser);

            _unitOfWork.Save();
        }

        public ResponseDto<string> SignUp(SignUpDto signUpDto)
        {
            if (UserAlsoExists(signUpDto.Cpf, signUpDto.Email))
                return new ResponseDto<string>(StatusCodes.Status400BadRequest, "O E-mail ou Cpf já estão cadastrados no sistema.");

            var newUser = _mapper.Map<User>(signUpDto);

            _unitOfWork.User.Add(newUser);

            if (_unitOfWork.Save() <= 0)
                return new ResponseDto<string>(StatusCodes.Status400BadRequest, "Não foi possível adicionar esse usuário ao sistema.");
            
            return new ResponseDto<string>(StatusCodes.Status201Created, "Usuário adicionado com sucesso!");
        }

        private bool UserAlsoExists(string cpf, string email)
        {
            var user = _unitOfWork.User.FindOne(x => x.Cpf == cpf || x.Email == email);

            return user != null;
        }

        public string UpdateTokens(int subUserId)
        {
            var refreshToken = _context?.HttpContext?.Request.Cookies["refreshToken"];
            var currentUser = _unitOfWork.User.FindOne(x => x.Id == subUserId);

            if (!currentUser.RefreshToken.Equals(refreshToken))
            {
                throw new UnauthorizedAccessException("Você não possui acesso ao sistema.");
            }

            if (currentUser.TokenExpires < DateTime.Now)
            {
                throw new UnauthorizedAccessException("Seu tempo de sessão acabou. Entre novamente.");
            }

            var userToken = _mapper.Map<UserTokenDto>(currentUser);
            string newToken = _tokenService.Generate(userToken);

            var newRefreshToken = _tokenService.GenerateRefresh();
            SetRefreshToken(newRefreshToken, subUserId);

            return newToken;
        }
    }
}
