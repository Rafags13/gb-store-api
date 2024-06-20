namespace GbStoreApi.Application.Extensions
{
    public class AmazonSecretsClass
    {
        public required string AccessKeyId { get; set; }
        public required string SecretAccessKey { get; set; }
        public required string SecretRegion { get; set; }
    }
}
