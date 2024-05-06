using Amazon.S3;
using Amazon.S3.Model;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.AmazonBuckets;
using Microsoft.AspNetCore.Http;

namespace GbStoreApi.Application.Services.AmazonBuckets
{
    public class FileService : IFileService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IBucketService _bucketService;
        public FileService(IAmazonS3 s3Client, IBucketService bucketService)
        {
            _s3Client = s3Client;
            _bucketService = bucketService;
        }

        public async Task<string> CreateFile(IFormFile formFile, string bucketName, string? prefix)
        {

            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? formFile.FileName : $"{prefix?.TrimEnd('/')}/{formFile.FileName}",
                InputStream = formFile.OpenReadStream()
            };

            request.Metadata.Add("Content-Type", formFile.ContentType);
            await _s3Client.PutObjectAsync(request);

            return formFile.FileName;
        }

        private string CreateFileSync(IFormFile formFile, string bucketName, string? prefix)
        {
            var randomNameFromFile = GenerateRandomName();
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? randomNameFromFile : $"{prefix?.TrimEnd('/')}/{randomNameFromFile}",
                InputStream = formFile.OpenReadStream()
            };

            request.Metadata.Add("Content-Type", formFile.ContentType);
            var response = _s3Client.PutObjectAsync(request).Result;

            return randomNameFromFile;
        }

        public async Task<List<string>> CreateMultipleFiles(IEnumerable<IFormFile> files, string? prefix)
        {
            var bucketName = await _bucketService.GetCurrentPictureBucket();
            var pictureNames = new List<string>();

            files.ToList().ForEach(file =>
            {
                var fileName = CreateFileSync(file, bucketName, "");
                pictureNames.Add(fileName);
            });

            return pictureNames;
        }

        private static string GenerateRandomName()
        {
            long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            Guid guid = Guid.NewGuid();

            string randomName = $"{timestamp}_{guid}";

            return randomName;
        }

        public async Task<IEnumerable<S3ObjectDto>> GetAllFilesAsync(string bucketName, string? prefix)
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Prefix = prefix
            };

            var result = await _s3Client.ListObjectsV2Async(request);

            var s3Objects = result.S3Objects.Select(s3 =>
            {
                var urlRequest = new GetPreSignedUrlRequest()
                {
                    BucketName = s3.BucketName,
                    Key = s3.Key,
                    Expires = DateTime.UtcNow.AddDays(1)
                };

                return new S3ObjectDto()
                {
                    Name = s3.Key.ToString(),
                    Url = _s3Client.GetPreSignedURL(urlRequest)
                };
            });

            return s3Objects;
        }

        public async Task<GetObjectResponse> GetFile(string bucketName, string key)
        {
            var request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = key
            };

            return await _s3Client.GetObjectAsync(request);
        }

        public async Task<S3ObjectDto> GetFileWithUrl(string bucketName, string key)
        {
            var result = await _s3Client.GetObjectAsync(bucketName, key);

            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = result.BucketName,
                Key = result.Key,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            var s3Object = new S3ObjectDto()
            {
                Name = result.Key.ToString(),
                Url = _s3Client.GetPreSignedURL(urlRequest),
            };

            return s3Object;
        }
    }
}
