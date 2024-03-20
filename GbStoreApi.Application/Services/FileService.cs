using Amazon.S3;
using Amazon.S3.Model;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IAmazonS3 _s3Client;
        public FileService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
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
