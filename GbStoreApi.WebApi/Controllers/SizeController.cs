using GbStoreApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Authorize]
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

        [HttpPost]
        public IActionResult Create([FromBody] string sizeName)
        {
            var response = _sizeService.Create(sizeName);
            return StatusCode(response.StatusCode, response);
        }
    }
}
