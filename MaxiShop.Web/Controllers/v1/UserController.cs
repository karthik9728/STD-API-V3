using Azure;
using MaxiShop.Application.ApplicationConstants;
using MaxiShop.Application.Common;
using MaxiShop.Application.DTO.Product;
using MaxiShop.Application.InputModels;
using MaxiShop.Application.Services;
using MaxiShop.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MaxiShop.Web.Controllers.v1
{
    [Route("api/v{version:apiVersion}/User")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected APIResponse _response;

        public UserController(IAuthService authService)
        {
            _authService = authService;
            _response = new APIResponse();
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<APIResponse>> Register(Register register)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    _response.AddError(ModelState.ToString());
                    _response.AddWarning(CommonMessage.RegistrationFailed);
                    return _response;
                }

                var result = await _authService.Register(register);

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.DisplayMessage = CommonMessage.RegistrationSuccess;
                _response.Result = result;
            }
            catch (Exception)
            {
                _response.AddError(CommonMessage.SystemError);
            }

            return _response;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<APIResponse>> Login(Login login)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    _response.AddError(ModelState.ToString());
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.AddWarning(CommonMessage.LoginFailed);
                    return _response;
                }

                var result = await _authService.Login(login);

                if (result is string)
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.DisplayMessage = CommonMessage.LoginFailed;
                    _response.Result = result;
                    return _response;
                }

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.DisplayMessage = CommonMessage.LoginSuccess;
                _response.Result = result;
            }
            catch (Exception ex)
            {
                _response.AddError(CommonMessage.SystemError + ex.Message.ToString());
            }

            return _response;
        }
    }
}
