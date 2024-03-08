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
        public IActionResult Create([FromBody] CreateProductDto createProductDto)
        {
            try
            {
                var success = _productService.CreateProduct(createProductDto);

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

        [HttpGet("Get-Catalog")]
        public IActionResult GetCatalog(
            [FromQuery(Name="category")] string category,
            [FromQuery(Name="brand")] string brand,
            [FromQuery(Name="Cores")] string colors,
            [FromQuery(Name="Tamanhos")] string sizes
            )
        {
            try
            {
                var parameters = new QuerySearchCatalogDto { Category = category, Brand = brand, Colors = colors, Sizes = sizes };
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
