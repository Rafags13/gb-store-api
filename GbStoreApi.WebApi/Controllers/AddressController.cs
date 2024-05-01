﻿using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Address;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
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

        [HttpGet("GetAllByUser")]
        public IActionResult GetByUserId()
        {
            var response = _addressService.GetAllByUserId();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateAddressDto createAddressDto)
        {
            var response = _addressService.Create(createAddressDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{zipCode}")]
        public IActionResult Update([FromBody] UpdateAddressDto updateAddressDto, [FromRoute] string zipCode)
        {
            var response = _addressService.Update(updateAddressDto, zipCode);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{zipCode}")]
        public IActionResult Remove([FromRoute] string zipCode)
        {
            var response = _addressService.Remove(zipCode);
            return StatusCode(response.StatusCode, response);
        }
    }
}
