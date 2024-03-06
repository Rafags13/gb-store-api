using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using GbStoreApi.Domain.enums;
using GbStoreApi.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GbStoreApi.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly MyConfigurationClass _configuration;
        public TokenService(IOptions<MyConfigurationClass> configs)
        {
            _configuration = configs.Value;
        }
        public string Generate(UserTokenDto user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.PrivateKey);
            var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = credentials,
            };
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(UserTokenDto user)
        {
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            ci.AddClaim(new Claim(ClaimTypes.Role, user.TypeOfUser.ToString()));

            return ci;
        }

    }
}
