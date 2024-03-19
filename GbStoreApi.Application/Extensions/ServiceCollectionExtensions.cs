using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GbStoreApi.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomDefaultAWSOptions(this IServiceCollection serviceCollection, ConfigurationManager configuration)
        {
            var awsOptions = configuration.GetAWSOptions();
            awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
            _ = new EnvironmentVariableAWSRegion(); // This add region to aws
            serviceCollection.AddDefaultAWSOptions(awsOptions);
            return serviceCollection;
        }
    }
}
