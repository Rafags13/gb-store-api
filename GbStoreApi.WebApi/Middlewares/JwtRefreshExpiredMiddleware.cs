using GbStoreApi.Application.Extensions;
using GbStoreApi.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
                    ValidateLifetime = false,
                }, out validatedToken);

                if (validatedToken.ValidTo < DateTime.UtcNow)
                {
                    var subId = int.Parse(principal.Identities.FirstOrDefault().Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                    var newToken = _authenticationService.RefreshToken(subId);
                    context.Request.Headers.Remove("Token");
                    context.Request.Headers.Add("Token", new StringValues(newToken));
                    context.Response.Headers.Add("Token", new StringValues(newToken));
                }

                
            }
            await next(context);
        }

    }
}
