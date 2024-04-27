using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Colors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Authorize]
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
        public IActionResult GetAll()
        {
            var response = _colorService.GetAll();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var response = _colorService.GetById(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] string colorName)
        {
            var response = _colorService.CreateColor(colorName);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateColorDto updateColorDto)
        {
            var response = _colorService.Update(updateColorDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            var response = _colorService.Delete(id);

            return StatusCode(response.StatusCode, response);
        }

    }
}
