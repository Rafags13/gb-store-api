using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Colors;
using GbStoreApi.Domain.Dto.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("[controller]")]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;
        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<IEnumerable<DisplayColorDto>>))]
        public IActionResult GetAll()
        {
            var response = _colorService.GetAll();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayColorDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayColorDto>))]
        public IActionResult Get(int id)
        {
            var response = _colorService.GetById(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayColorDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<DisplayColorDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayColorDto>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseDto<DisplayColorDto>))]
        public IActionResult Create([FromBody] string colorName)
        {
            var response = _colorService.CreateColor(colorName);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayColorDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayColorDto>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseDto<DisplayColorDto>))]
        public IActionResult Update([FromBody] UpdateColorDto updateColorDto)
        {
            var response = _colorService.Update(updateColorDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayColorDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<DisplayColorDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayColorDto>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseDto<DisplayColorDto>))]
        public IActionResult Delete(int id) 
        {
            var response = _colorService.Delete(id);

            return StatusCode(response.StatusCode, response);
        }

    }
}
