using MaxiShop.Application.ApplicationConstants;
using MaxiShop.Application.Common;
using MaxiShop.Application.DTO.Product;
using MaxiShop.Application.InputModels;
using MaxiShop.Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Web.Controllers.v1
{
    [Route("api/v{version:apiVersion}/Product")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        protected APIResponse _response;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
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
                var products = await _productService.GetAll();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = products;

                _logger.LogInformation("Product Controller - All Products Fetched");
            }
            catch (Exception)
            {
                _response.AddError(CommonMessage.SystemError);
            }

            return _response;
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        [Route("CSVReport")]
        public async Task<ActionResult<APIResponse>> GetReport()
        {
            try
            {
                var products = await _productService.GetAll();

                static string GetPropertyDisplayName(PropertyInfo propertyInfo)
                {
                    var displayNameAttribute = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();
                    return displayNameAttribute != null ? displayNameAttribute.DisplayName : propertyInfo.Name;
                }

                List<ProductDto> data = products.ToList();

                if (data == null || data.Count == 0)
                    return BadRequest("No data provided.");

                StringBuilder csvBuilder = new StringBuilder();

                // Get the header row from the first object in the list
                var properties = data[0].GetType().GetProperties();
                //var headerRow = string.Join(",", properties.Select(p => p.Name));
                var headerRow = string.Join(",", properties.Select(GetPropertyDisplayName));
                csvBuilder.AppendLine(headerRow);

                // Add data rows
                foreach (var obj in data)
                {
                    var dataRow = string.Join(",", properties.Select(p => p.GetValue(obj)?.ToString() ?? ""));
                    csvBuilder.AppendLine(dataRow);
                }

                var csvData = csvBuilder.ToString();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;

                // Return the CSV data as a response
                return File(Encoding.UTF8.GetBytes(csvData), "text/csv", $"Product{DateTime.UtcNow.ToString("dd-MM-yyyy")}.csv");

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
