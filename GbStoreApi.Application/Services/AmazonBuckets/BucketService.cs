using Amazon.S3;
using GbStoreApi.Application.Interfaces;

namespace GbStoreApi.Application.Services.AmazonBuckets
{
    public class BucketService : IBucketService
    {
        private readonly IAmazonS3 _amazonS3Service;
        public BucketService(IAmazonS3 amazonS3Service)
        {
            _amazonS3Service = amazonS3Service;
        }
        public async Task<IEnumerable<string>> GetAllBuckets()
        {
            var data = await _amazonS3Service.ListBucketsAsync();
            var buckets = data.Buckets.Select(bucket => { return bucket.BucketName; });

            return buckets;
        }

        public async Task<string?> GetCurrentPictureBucket()
        {
            var data = await _amazonS3Service.ListBucketsAsync();
            var currentBucket = data.Buckets.FirstOrDefault(bucket => bucket.BucketName.Contains("picture"))?.BucketName;

            return currentBucket;
        }
    }
}
