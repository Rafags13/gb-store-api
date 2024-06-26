using GbStoreApi.Application.Extensions;
using GbStoreApi.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GbStoreApi.Application.Middlewares
{
    public class JwtRefreshExpiredMiddleware : IMiddleware
    {
        private readonly MyConfigurationClass _jwtSettings;
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public JwtRefreshExpiredMiddleware(
            IOptions<MyConfigurationClass> jwtSettings,
            Lazy<IAuthenticationService> authenticationService
            )
        {
            _jwtSettings = jwtSettings.Value;
            _authenticationService = authenticationService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (context.Request.Method.Equals("OPTIONS"))
                {
                    await next(context);
                    return;
                }

                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (!string.IsNullOrEmpty(token))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_jwtSettings.PrivateKey);

                    var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                    }, out SecurityToken validatedToken);

                    if (validatedToken.ValidTo < DateTime.UtcNow)
                    {
                        if(!int.TryParse(principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value, out int subId))
                            throw new UnauthorizedAccessException("Usuário Inválido.");

                        var response = _authenticationService.Value.UpdateTokens(subId);

                        if (response.StatusCode != StatusCodes.Status200OK)
                            throw new UnauthorizedAccessException(response.Message);

                        context.Request.Headers.Remove("Token");
                        context.Request.Headers.Add("Token", new StringValues(response.Value));
                        context.Response.Headers.Add("Token", new StringValues(response.Value));
                    }
                }
                await next(context);

                
            }
            catch (Exception ex) when (ex is SecurityTokenSignatureKeyNotFoundException || ex is UnauthorizedAccessException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await next(context);
            }
        }

    }
}
