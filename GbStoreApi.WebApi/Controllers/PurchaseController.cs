using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Purchases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Common")]
    [ApiController]

    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<bool>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseDto<bool>))]
        public IActionResult BuyProduct([FromBody] BuyProductDto buyProductDto)
        {
            var response = _purchaseService.BuyProduct(buyProductDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<IEnumerable<PurchaseSpecificationDto>>))]
        public IActionResult GetAll()
        {
            var response = _purchaseService.GetAll();
            return StatusCode(response.StatusCode, response);
        }
    }
}
