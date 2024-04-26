﻿using GbStoreApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _brandService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]

        public IActionResult Create([FromBody] string brandName)
        {
            var response = _brandService.Create(brandName);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var response = _brandService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{name}")]
        public IActionResult GetById([FromRoute] string name)
        {
            var response = _brandService.GetByName(name);
            return StatusCode(response.StatusCode, response);
        }
    }
}
