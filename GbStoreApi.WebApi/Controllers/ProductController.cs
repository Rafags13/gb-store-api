using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Product.Catalogs;
using GbStoreApi.Domain.Dto.Products;
using GbStoreApi.Domain.Dto.Stocks;
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var response = _productService.GetAll();

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("Filters/{page}")]
        public IActionResult GetByFilters(
            [FromQuery] string[]? Tamanhos,
            [FromQuery] string[]? Cores,
            [FromQuery] string? Category,
            [FromRoute] int page = 0
            )
        {
            try
            {
                var filters = new CatalogFilterDto { 
                    Category = Category,
                    Cores = Cores,
                    Tamanhos = Tamanhos,
                    Page = page,
                    PageSize = 20,
                };
                var response = _productService.GetByFilters(filters);
                return StatusCode(response.StatusCode, response);
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

        [AllowAnonymous]
        [HttpPost("GetAvaliableStocks")]
        public IActionResult GetAvaliableStocks([FromBody] IEnumerable<CountStockByItsIdDto> countStockByItsIdDtos)
        {
            try
            {
                var avaliableStocks = _productService.GetAvaliableStocks(countStockByItsIdDtos);

                return Ok(avaliableStocks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
