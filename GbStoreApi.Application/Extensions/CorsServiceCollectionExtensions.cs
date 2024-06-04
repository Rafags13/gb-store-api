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
                    builder => builder.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                    );
            });
            return serviceCollection;
        }
    }
}
