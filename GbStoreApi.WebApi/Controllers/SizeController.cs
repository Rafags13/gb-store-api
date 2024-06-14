using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Sizes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("[controller]")]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _sizeService;
        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<IEnumerable<DisplaySizeDto>>))]
        public IActionResult GetAll()
        {
            var response = _sizeService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplaySizeDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplaySizeDto>))]
        public IActionResult GetById([FromRoute] int id)
        {
            var response = _sizeService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplaySizeDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplaySizeDto>))]
        public IActionResult GetByName([FromRoute] string name)
        {
            var response = _sizeService.GetByName(name);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplaySizeDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplaySizeDto>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseDto<DisplaySizeDto>))]
        public IActionResult Create([FromBody] string sizeName)
        {
            var response = _sizeService.Create(sizeName);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplaySizeDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<DisplaySizeDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplaySizeDto>))]
        public IActionResult Update([FromBody] UpdateSizeDto updateSizeDto)
        {
            var response = _sizeService.Update(updateSizeDto);
            return StatusCode(response.StatusCode, updateSizeDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplaySizeDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<DisplaySizeDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplaySizeDto>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseDto<DisplaySizeDto>))]
        public IActionResult Delete([FromRoute] int id)
        {
            var response = _sizeService.Delete(id);
            return StatusCode(response.StatusCode, id);
        }
    }
}
