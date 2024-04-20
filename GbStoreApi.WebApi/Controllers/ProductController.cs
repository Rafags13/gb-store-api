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

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var products = _productService.GetAll();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("Filters")]
        public IActionResult GetByFilters(
            [FromQuery] string[]? Tamanhos,
            [FromQuery] string[]? Cores,
            [FromQuery] string? Category
            )
        {
            try
            {
                var filters = new CatalogFilterDto { Category = Category, Cores = Cores, Tamanhos = Tamanhos};
                var products = _productService.GetByFilters(filters);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto createProductDto)
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

        [AllowAnonymous]
        [HttpGet("Informations/{productId:int}")]
        public IActionResult GetProductSpecificationById([FromRoute] int productId)
        {
            try
            {
                var currentProduct = _productService.GetProductSpecificationById(productId);

                return Ok(currentProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("GetAllFilters")]
        public IActionResult GetAllFilters()
        {
            try
            {
                var allFilters = _productService.GetAllFilters();

                return Ok(allFilters);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
