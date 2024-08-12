using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace GbStoreApi.Application.Extensions
{
    public static class CorsServiceCollectionExtensions
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection serviceCollection, string corsPolicyName)
        {
            serviceCollection.AddCors(option =>
            {
                option.AddPolicy(corsPolicyName,
                    builder => builder.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .WithExposedHeaders("Token")
                        .WithHeaders(
                            HeaderNames.AccessControlAllowCredentials,
                            HeaderNames.AccessControlAllowOrigin,
                            HeaderNames.ContentType,
                            HeaderNames.Authorization)
                        .AllowCredentials()
                    );
            });
            return serviceCollection;
        }
    }
}
