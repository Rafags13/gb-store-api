using Microsoft.AspNetCore.Mvc;
using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Authentications;
using GbStoreApi.Domain.Dto.Generic;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<string>))]
        public IActionResult SignIn([FromBody] SignInDto signInDto)
        {
            var response = _authenticationService.SignIn(signInDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseDto<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<string>))]
        public IActionResult SignUp([FromBody] SignUpDto signUpDto)
        {
            var response = _authenticationService.SignUp(signUpDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("Refresh-Token/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult Refresh(int userId)
        {
            try
            {
                var newToken = _authenticationService.UpdateTokens(userId);

                return Ok(newToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
