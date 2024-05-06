using GbStoreApi.Application.Interfaces;
using GbStoreApi.Domain.Dto.Categories;
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
        public IActionResult GetAll()
        {
            var response = _categoryService.GetAll();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id) 
        {
            var response = _categoryService.GetById(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] string categoryName)
        {
            var response = _categoryService.Create(categoryName);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateCategoryDto updateCategoryDto)
        {
            var response = _categoryService.Update(updateCategoryDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var response = _categoryService.Delete(id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
