using Amazon.S3.Model;
using GbStoreApi.Domain.Dto.AmazonBuckets;
using GbStoreApi.Domain.Dto.Generic;
using Microsoft.AspNetCore.Http;

namespace GbStoreApi.Application.Interfaces
{
    public interface IFileService
    {
        Task<ResponseDto<string>> CreateFileAsync(IFormFile formFile, string bucketName, string? prefix);
        Task<List<string>> CreateMultipleFiles(IEnumerable<IFormFile> files, string? prefix = "");
        Task<ResponseDto<GetObjectResponse>> GetFile(string bucketName, string key);
        Task<ResponseDto<IEnumerable<S3ObjectDto>>> GetAllFilesAsync(string bucketName, string? prefix);
        Task<ResponseDto<S3ObjectDto>> GetFileWithUrl(string bucketName, string key);
    }
}
