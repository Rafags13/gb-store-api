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
            try
            {
                var filesWithUrl = await _fileService.GetAllFilesAsync(bucketName, prefix);

                if(filesWithUrl == null) return NotFound("Não existe nenhum item no catálogo ainda.");

                return Ok(filesWithUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{bucketName}/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> GetFileById(string bucketName, string key)
        {
            try
            {
                var file = await _fileService.GetFile(bucketName, key);

                if (file == null) return NotFound("O arquivo informado não existe no sistema.");

                return File(file.ResponseStream, file.Headers.ContentType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("With-Url")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(S3ObjectDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> GetFileWithUrl(string bucketName, string key)
        {
            try
            {
                var fileWithUrl = await _fileService.GetFileWithUrl(bucketName, key);

                if (fileWithUrl == null) return NotFound("O arquivo informado não existe no sistema.");

                return Ok(fileWithUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UploadFileAsync(IFormFile formFile, string bucketName, string? prefix)
        {
            try
            {
                var fileName = await _fileService.CreateFile(formFile, bucketName, prefix);

                return Ok($"File {fileName} uploaded s3 successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
