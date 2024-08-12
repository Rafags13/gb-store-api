using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Categories;
using GbStoreApi.Domain.Dto.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GbStoreApi.WebApi.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Administrator")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<IEnumerable<DisplayCategoryDto>>))]
        public IActionResult GetAll()
        {
            var response = _categoryService.GetAll();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        public IActionResult GetById(int id) 
        {
            var response = _categoryService.GetById(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        public IActionResult Create([FromBody] string categoryName)
        {
            var response = _categoryService.Create(categoryName);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        public IActionResult Update([FromBody] UpdateCategoryDto updateCategoryDto)
        {
            var response = _categoryService.Update(updateCategoryDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDto<DisplayCategoryDto>))]
        public IActionResult Delete([FromRoute] int id)
        {
            var response = _categoryService.Delete(id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
