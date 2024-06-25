using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Purchases;
using GbStoreApi.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Administrator,Common")]
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

        [HttpGet("Paginated")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<IEnumerable<AdminPurchaseDisplay>>))]
        public IActionResult GetPaginated(
            [FromQuery] string searchQuery = "",
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 20)
        {
            var response = _purchaseService.GetPaginated(
                searchQuery,
                page,
                pageSize
                );
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Specification/{id:int}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<AdminPurchaseSpecificationDto>))]
        public IActionResult GetSpecificationById(
            [FromRoute] int id
            )
        {
            var response = _purchaseService.GetSpecificationById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<bool>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<bool>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseDto<bool>))]
        public IActionResult UpdateStateById(
            [FromRoute] int id,
            [FromBody] PurchaseState newState
            )
        {
            var response = _purchaseService.UpdateStateById(id, newState);
            return StatusCode(response.StatusCode, response);
        }
    }
}
