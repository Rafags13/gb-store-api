using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GbStoreApi.WebApi.Middlewares
{
    public class JwtRefreshExpiredMiddleware : IMiddleware
    {
        private readonly MyConfigurationClass _jwtSettings;
        private readonly IAuthenticationService _authenticationService;

        public JwtRefreshExpiredMiddleware(
            IOptions<MyConfigurationClass> jwtSettings,
            IAuthenticationService authenticationService
            )
        {
            _jwtSettings = jwtSettings.Value;
            _authenticationService = authenticationService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.PrivateKey);
                
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                }, out validatedToken);

                // Se o token estiver expirado
                if (validatedToken.ValidTo < DateTime.UtcNow)
                {
                    var newToken = _authenticationService.RefreshToken();
                    context.Response.Headers.Add("Token", new StringValues(newToken));
                }
            }

            await next(context);
        }

    }
}
