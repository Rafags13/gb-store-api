using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GbStoreApi.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomDefaultAWSOptions(this IServiceCollection serviceCollection, ConfigurationManager configuration)
        {
            var awsOptions = configuration.GetAWSOptions();

            var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            awsOptions.Credentials = new BasicAWSCredentials(accessKey, secretKey);
            awsOptions.Region = RegionEndpoint.SAEast1;

            serviceCollection.AddDefaultAWSOptions(awsOptions);
            return serviceCollection;
        }
    }
}
