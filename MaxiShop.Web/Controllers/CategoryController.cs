using MaxiShop.Application.DTO.Category;
using MaxiShop.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MaxiShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var categories = await _categoryService.GetAll();

            return Ok(categories);
        }

        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult> Details(int id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _categoryService.Create(dto);

            return CreatedAtAction(nameof(Details), new { id = category.Id }, category);
        }

        [HttpPut]
        public async Task<ActionResult> Update(UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _categoryService.GetById(dto.Id);

            if (category == null)
            {
                return NotFound("Record Not Found");
            }

            await _categoryService.Update(dto);

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([Required] int id)
        {
            if (id == 0)
            {
                return BadRequest(ModelState);
            }

            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound("Record Not Found");
            }

            await _categoryService.Delete(id);

            return NoContent();
        }
    }
}
