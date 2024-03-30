using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
        {
            try
            {
                var success = await _productService.CreateProduct(createProductDto);

                if(success)
                {
                    return Ok("Produto criado com sucesso!");
                }

                return BadRequest("Não foi possível criar o produto.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Current-Variants")]
        public IActionResult GetCurrentVariants()
        {
            try
            {
                var currentVariants = _productService.GetCurrentVariants();

                return Ok(currentVariants);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
