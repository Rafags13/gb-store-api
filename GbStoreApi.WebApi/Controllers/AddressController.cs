using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Address;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(
            IAddressService addressService
            )
        {
            _addressService = addressService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _addressService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var response = _addressService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("/GetAllByUser/{userId:int}")]
        public IActionResult GetByUserId(int userId)
        {
            var response = _addressService.GetAllByUserId(userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateAddressDto createAddressDto)
        {
            var response = _addressService.Create(createAddressDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
