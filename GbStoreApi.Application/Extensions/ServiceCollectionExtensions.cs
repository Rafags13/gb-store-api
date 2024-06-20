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
            var configurationAmazon = configuration.GetSection("Configuration").Get<MyConfigurationClass>()!.AmazonSecrets;

            awsOptions.Credentials = new BasicAWSCredentials(configurationAmazon.AccessKeyId, configurationAmazon.SecretAccessKey);
            awsOptions.Region = RegionEndpoint.SAEast1;

            serviceCollection.AddDefaultAWSOptions(awsOptions);
            return serviceCollection;
        }
    }
}
