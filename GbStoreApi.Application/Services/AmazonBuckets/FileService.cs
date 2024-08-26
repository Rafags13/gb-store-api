﻿using Amazon.S3;
using Amazon.S3.Model;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Constants;
using GbStoreApi.Domain.Dto.AmazonBuckets;
using GbStoreApi.Domain.Dto.Generic;
using Microsoft.AspNetCore.Http;

namespace GbStoreApi.Application.Services.AmazonBuckets
{
    public class FileService : IFileService
    {
        private readonly IAmazonS3 _s3Client;
        public FileService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<ResponseDto<string>> CreateFileAsync(IFormFile formFile, string bucketName, string? prefix)
        {
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? formFile.FileName : $"{prefix?.TrimEnd('/')}/{formFile.FileName}",
                InputStream = formFile.OpenReadStream()
            };

            request.Metadata.Add("Content-Type", formFile.ContentType);
            await _s3Client.PutObjectAsync(request);

            return new(formFile.FileName);
        }

        private string CreateFileSync(IFormFile formFile, string bucketName, string? prefix)
        {
            var randomNameFromFile = GenerateRandomName();
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? randomNameFromFile : $"{prefix}-{randomNameFromFile}",
                InputStream = formFile.OpenReadStream()
            };

            request.Metadata.Add("Content-Type", formFile.ContentType);
            _ = _s3Client.PutObjectAsync(request).Result;

            return randomNameFromFile;
        }

        public async Task<List<string>> CreateMultipleFiles(IEnumerable<IFormFile> files, string? prefix)
        {
            var pictureNames = new List<string>();

            Parallel.ForEach(files, file => {
                var fileName = CreateFileSync(file, BucketContants.BUCKET_S3_NAME, prefix);
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

        public async Task<ResponseDto<IEnumerable<S3ObjectDto>>> GetAllFilesAsync(string bucketName, string? prefix)
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Prefix = prefix,
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

            return new(s3Objects);
        }

        public async Task<ResponseDto<GetObjectResponse>> GetFile(string bucketName, string key)
        {
            var request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = key
            };

            var file = await _s3Client.GetObjectAsync(request);

            return new(file);
        }

        public async Task<ResponseDto<S3ObjectDto>> GetFileWithUrl(string bucketName, string key)
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

            return new(s3Object);
        }
    }
}
