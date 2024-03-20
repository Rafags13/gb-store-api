using Amazon.S3.Model;
using GbStoreApi.Domain.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GbStoreApi.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> CreateFile(IFormFile formFile, string bucketName, string? prefix);
        Task<GetObjectResponse> GetFile(string bucketName, string key);
        Task<IEnumerable<S3ObjectDto>> GetAllFilesAsync(string bucketName, string? prefix);
        Task<S3ObjectDto> GetFileWithUrl(string bucketName, string key);
    }
}
