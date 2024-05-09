using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Common")]
    [ApiController]

    public class PurchaseController : ControllerBase
    {
        public PurchaseController()
        {
            
        }

        
    }
}
