using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.AmazonBuckets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Administrator")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Task<IEnumerable<S3ObjectDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> GettAllFilesAsync(string bucketName, string? prefix)
        {
            var filesWithUrl = await _fileService.GetAllFilesAsync(bucketName, prefix);

            return StatusCode(StatusCodes.Status200OK, filesWithUrl);
        }

        [HttpGet("{bucketName}/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> GetFileById(string bucketName, string key)
        {
            var file = await _fileService.GetFile(bucketName, key);
            return StatusCode(file.StatusCode, file);

        }

        [HttpGet("With-Url")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(S3ObjectDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> GetFileWithUrl(string bucketName, string key)
        {
            var fileWithUrl = await _fileService.GetFileWithUrl(bucketName, key);
            return StatusCode(StatusCodes.Status200OK, fileWithUrl);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UploadFileAsync(IFormFile formFile, string bucketName, string? prefix)
        {
            var fileName = await _fileService.CreateFileAsync(formFile, bucketName, prefix);
            return StatusCode(StatusCodes.Status200OK, fileName);
        }
    }
}
