using MaxiShop.Application.DTO.Category;
using MaxiShop.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MaxiShop.Web.Controllers.v2
{
    [Route("api/v{version:apiVersion}/Category")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [ProducesResponseType(200)]
        //[ResponseCache(Duration = 30)]
        [ResponseCache(CacheProfileName = "Default30")]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var categories = await _categoryService.GetAll();

            return Ok(categories);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
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
    }
}
