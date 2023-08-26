using MaxiShop.Application.DTO.Brand;
using MaxiShop.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MaxiShop.Web.Controllers.v1
{
    [Route("api/v{version:apiVersion}/Brand")]
    [ApiVersion("1.0")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }


        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var brands = await _brandService.GetAll();

            return Ok(brands);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult> Details(int id)
        {
            var brand = await _brandService.GetById(id);

            if (brand == null)
            {
                return NotFound();
            }

            return Ok(brand);
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Create(CreateBrandDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brand = await _brandService.Create(dto);

            return CreatedAtAction(nameof(Details), new { id = brand.Id }, brand);
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<ActionResult> Update(UpdateBrandDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brand = await _brandService.GetById(dto.Id);

            if (brand == null)
            {
                return NotFound("Record Not Found");
            }

            await _brandService.Update(dto);

            return NoContent();
        }



        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<ActionResult> Delete([Required] int id)
        {
            if (id == 0)
            {
                return BadRequest(ModelState);
            }

            var brand = await _brandService.GetById(id);

            if (brand == null)
            {
                return NotFound("Record Not Found");
            }

            await _brandService.Delete(id);

            return NoContent();
        }
    }
}
