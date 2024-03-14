namespace GbStoreApi.WebApi.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseFactoryActivatedMiddleware(
            this IApplicationBuilder app)
            => app.UseMiddleware<JwtRefreshExpiredMiddleware>();
    }
}
