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
            var response = _productService.GetAll();

            return StatusCode(response.StatusCode, response);
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
            
            var response = await _productService.CreateProduct(createProductDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Current-Variants")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DisplayVariantsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult GetCurrentVariants()
        {
            var currentVariants = _productService.GetCurrentVariants();
            return StatusCode(StatusCodes.Status200OK, currentVariants);
        }

        [HttpGet("Informations/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductSpecificationsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult GetProductSpecificationById([FromRoute] int productId)
        {
            var currentProduct = _productService.GetProductSpecificationById(productId);
            return StatusCode(StatusCodes.Status200OK, currentProduct);
        }

        [HttpGet("GetAllFilters")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DisplayFiltersDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult GetAllFilters()
        {
            var allFilters = _productService.GetAllFilters();
            return StatusCode(StatusCodes.Status200OK, allFilters);
        }

        [HttpPost("GetAvaliableStocks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StockAvaliableByIdDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult GetAvaliableStocks([FromBody] IEnumerable<CountStockByItsIdDto> countStockByItsIdDtos)
        {
           
            var avaliableStocks = _productService.GetAvaliableStocks(countStockByItsIdDtos);
            return StatusCode(StatusCodes.Status200OK, avaliableStocks);
        }
    }
}
