using MaxiShop.Application.ApplicationConstants;
using MaxiShop.Application.Common;
using MaxiShop.Application.DTO.Product;
using MaxiShop.Application.InputModels;
using MaxiShop.Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MaxiShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        protected APIResponse _response;

        public ProductController(IProductService productService)
        {
            _productService = productService;
            _response = new APIResponse();
        }

        [ProducesResponseType(200)]
        [HttpPost]
        [Route("GetPagination")]
        public async Task<ActionResult<APIResponse>> GetPagination([FromBody] PaginationIP paginationInput)
        {
            try
            {
                var categories = await _productService.GetPagination(paginationInput);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = categories;
            }
            catch (Exception)
            {
                _response.AddError(CommonMessage.SystemError);
            }

            return _response;
        }

        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<ActionResult<APIResponse>> Get()
        {
            try
            {
                var categories = await _productService.GetAll();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = categories;
            }
            catch (Exception)
            {
                _response.AddError(CommonMessage.SystemError);
            }

            return _response;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("Details")]
        public async Task<ActionResult<APIResponse>> Details(int id)
        {
            try
            {
                var product = await _productService.GetById(id);

                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.AddWarning(CommonMessage.RecordNotFound);
                }

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = product;

            }
            catch (Exception)
            {
                _response.AddError(CommonMessage.SystemError);
            }

            return _response;
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> Create(CreateProductDto dto)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    _response.AddError(ModelState.ToString());
                    _response.AddWarning(CommonMessage.CreateOperationFailed);
                    return _response;
                }

                var product = await _productService.Create(dto);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.DisplayMessage = CommonMessage.CreateOperationSuccess;
                _response.Result = product;
            }
            catch (Exception)
            {

                _response.AddError(CommonMessage.SystemError);
            }

            return _response;
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<ActionResult<APIResponse>> Update(UpdateProductDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.AddError(ModelState.ToString());
                    _response.AddWarning(CommonMessage.UpdateOperationFailed);
                    return _response;
                }

                var product = await _productService.GetById(dto.Id);

                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.AddError(CommonMessage.RecordNotFound);
                    _response.AddWarning(CommonMessage.UpdateOperationFailed);
                    return _response;
                }

                await _productService.Update(dto);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.DisplayMessage = CommonMessage.UpdateOperationSuccess;

            }
            catch (Exception)
            {
                _response.AddError(CommonMessage.SystemError);
            }

            return _response;
        }



        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        public async Task<ActionResult<APIResponse>> Delete([Required] int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.AddWarning(CommonMessage.DeleteOperationFailed);
                    return _response;
                }

                var product = await _productService.GetById(id);

                if (product == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.AddError(CommonMessage.RecordNotFound);
                    _response.AddWarning(CommonMessage.DeleteOperationFailed);
                    return _response;
                }

                await _productService.Delete(id);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.DisplayMessage = CommonMessage.DeleteOperationSuccess;
            }
            catch (Exception)
            {

                _response.AddError(CommonMessage.SystemError);
            }

            return _response;
        }
    }
}
