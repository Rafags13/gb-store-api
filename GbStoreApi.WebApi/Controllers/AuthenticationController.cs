using Microsoft.AspNetCore.Mvc;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Authentications;

namespace GbStoreApi.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public IActionResult SignIn([FromBody] SignInDto signInDto)
        {
            var response = _authenticationService.SignIn(signInDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("Register")]
        public IActionResult SignUp([FromBody] SignUpDto signUpDto)
        {
            var response = _authenticationService.SignUp(signUpDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("Refresh-Token/{userId}")]
        public IActionResult Refresh(int userId)
        {
            try
            {
                var newToken = _authenticationService.RefreshToken(userId);

                return Ok(newToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
