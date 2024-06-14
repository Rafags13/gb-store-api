using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Generic;
using GbStoreApi.Domain.Dto.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Authorize(Roles = "Common,Administrator")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(
            IUserService userService
            )
        {
            _userService = userService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<IEnumerable<DisplayUserDto>>))]
        public IActionResult GetAll()
        {
            var response = _userService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayUserDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayUserDto>))]
        public IActionResult GetById(int id) 
        {
            var response = _userService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Current-Informations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayUserDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseDto<DisplayUserDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayUserDto>))]
        public IActionResult GetCurrentInformations()
        {
            var response = _userService.GetCurrentInformations();
            return StatusCode(response.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpGet("Current-Role")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<string?>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<string?>))]
        public IActionResult GetCurrentRole()
        {
            var response = _userService.GetUserRole();
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Common,Administrator")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<bool>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<bool>))]
        public IActionResult Update([FromBody] UpdateUserDto updateDto)
        {
            var response = _userService.Update(updateDto);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Common,Administrator")]
        [HttpPut("Password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<bool>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseDto<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<bool>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<bool>))]
        public IActionResult UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            var response = _userService.UpdatePassword(updatePasswordDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
