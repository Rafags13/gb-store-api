using GbStoreApi.Application.Interfaces;
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
        public IActionResult BuyProduct([FromBody] BuyProductDto buyProductDto)
        {
            var response = _purchaseService.BuyProduct(buyProductDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
