using Microsoft.AspNetCore.Builder;

namespace GbStoreApi.Application.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseFactoryActivatedMiddleware(
            this IApplicationBuilder app)
            => app.UseMiddleware<JwtRefreshExpiredMiddleware>();
    }
}
