using GbStoreApi.Application.Interfaces;
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
        public IActionResult GetAll()
        {
            var response = _sizeService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var response = _sizeService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{name}")]
        public IActionResult GetById([FromRoute] string name)
        {
            var response = _sizeService.GetByName(name);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] string sizeName)
        {
            var response = _sizeService.Create(sizeName);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateSizeDto updateSizeDto)
        {
            var response = _sizeService.Update(updateSizeDto);
            return StatusCode(response.StatusCode, updateSizeDto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var response = _sizeService.Delete(id);
            return StatusCode(response.StatusCode, id);
        }
    }
}
