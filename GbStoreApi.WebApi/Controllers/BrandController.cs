using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Brands;
using GbStoreApi.Domain.Dto.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Administrator")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<IEnumerable<DisplayBrandDto>>))]
        public IActionResult GetAll()
        {
            var response = _brandService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayBrandDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayBrandDto>))]
        public IActionResult GetById([FromRoute] int id)
        {
            var response = _brandService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayBrandDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayBrandDto>))]
        public IActionResult GetByName([FromRoute] string name)
        {
            var response = _brandService.GetByName(name);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayBrandDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<DisplayBrandDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayBrandDto>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseDto<DisplayBrandDto>))]
        public IActionResult Create([FromBody] string brandName)
        {
            var response = _brandService.Create(brandName);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayBrandDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayBrandDto>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseDto<DisplayBrandDto>))]
        public IActionResult Update([FromBody] UpdateBrandDto updateBrandDto)
        {
            var response = _brandService.Update(updateBrandDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayBrandDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<DisplayBrandDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayBrandDto>))]
        public IActionResult Delete([FromRoute] int id)
        {
            var response = _brandService.Delete(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
