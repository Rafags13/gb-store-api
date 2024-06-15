using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Product.Catalogs;
using GbStoreApi.Domain.Dto.Products;
using GbStoreApi.Domain.Dto.Stocks;
using GbStoreApi.Domain.Dto.Stocks.Filters;
using GbStoreApi.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<IEnumerable<DisplayProductDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<IEnumerable<DisplayProductDto>>))]
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

        [HttpGet("Filters/{page}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<IEnumerable<DisplayProductDto>>))]
        public async Task<IActionResult> GetByFilters(
            [FromQuery] string[]? Sizes,
            [FromQuery] string[]? Colors,
            [FromQuery] string? Category,
            [FromQuery] string? OrderBy,
            [FromQuery] string? Direction,
            [FromRoute] int page = 0
            )
        {
            var filters = new CatalogFilterDto { 
                Category = Category,
                Colors = Colors,
                Sizes = Sizes,
                Page = page,
                Direction = Direction,
                OrderBy = OrderBy,
                PageSize = 20,
            };
            var response = await _productService.GetByFilters(filters);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DisplayVariantsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
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

        [HttpGet("Informations/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductSpecificationsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
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

        [HttpGet("GetAllFilters")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DisplayFiltersDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
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

        [HttpPost("GetAvaliableStocks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StockAvaliableByIdDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
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
