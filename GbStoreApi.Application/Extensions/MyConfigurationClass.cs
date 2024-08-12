namespace GbStoreApi.Application.Extensions
{
    public class MyConfigurationClass
    {
        public string PrivateKey { get; set; } = string.Empty;
        public AmazonSecretsClass AmazonSecrets { get; set; } = null!;
    }
}