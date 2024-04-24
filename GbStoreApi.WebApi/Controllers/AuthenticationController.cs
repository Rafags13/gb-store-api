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
            try
            {
                var token = _authenticationService.SignIn(signInDto);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        public IActionResult SignUp([FromBody] SignUpDto signUpDto)
        {
            try
            {
                var message = _authenticationService.SignUp(signUpDto);

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Refresh-Token/{userId}")]
        public IActionResult Refresh(int subUserId)
        {
            try
            {
                var newToken = _authenticationService.RefreshToken(subUserId);

                return Ok(newToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
