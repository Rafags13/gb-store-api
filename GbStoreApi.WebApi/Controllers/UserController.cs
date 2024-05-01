using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Authorize]
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

        
        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _userService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
        {
            var response = _userService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Current-Informations")]
        public IActionResult GetCurrentInformations()
        {
            var response = _userService.GetCurrentInformations();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Current-Role")]
        public IActionResult GetCurrentRole()
        {
            var response = _userService.GetUserRole();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateUserDto updateDto)
        {
            var response = _userService.Update(updateDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
