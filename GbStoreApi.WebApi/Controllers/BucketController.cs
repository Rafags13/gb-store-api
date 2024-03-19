using Amazon;
using Amazon.S3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BucketController : ControllerBase
    {
        private readonly IAmazonS3 _amazonS3Service;

        public BucketController(IAmazonS3 amazonS3Service)
        {
            _amazonS3Service = amazonS3Service;
        }

        [HttpGet]
        public async Task<IActionResult> ListAllBuckets()
        {   
            var data = await _amazonS3Service.ListBucketsAsync();
            var buckets = data.Buckets.Select(bucket => { return bucket.BucketName; });

            return Ok(buckets);
        }
    }
}
