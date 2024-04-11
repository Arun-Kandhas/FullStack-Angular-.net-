using KnilServer.Application.ApplicationConstant;
using KnilServer.Application.Common;
using KnilServer.Application.DTO.Contacts;
using KnilServer.Application.InputModel;
using KnilServer.Application.Services;
using KnilServer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KnilServer.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;
        protected APIResponse _apiResponse;

        public AuthController(IAuthService authService)
        {
            _authservice = authService;
            _apiResponse = new APIResponse();
        }


        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<APIResponse>> Registeration([FromBody] Register request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.AddError(ModelState.ToString()!);
                    return _apiResponse;
                }


                var result = await _authservice.Register(request);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = result;
                _apiResponse.DisplayMessage = "Success";

            }
            catch (Exception)
            {
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.AddError(CommonMessage.SystemError);
                return _apiResponse;

            }
            return Ok(_apiResponse);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<APIResponse>> Login(Login login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.AddError(ModelState.ToString()!);
                    return _apiResponse;
                }


                var Result = await _authservice.Login(login);

                if (Result is string)
                {
                    _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.DisplayMessage = "Login Failed";
                    _apiResponse.Result = Result;
                    return _apiResponse;
                }

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.DisplayMessage = "Login Successfully";
                _apiResponse.Result = Result;

            }
            catch (Exception)
            {
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.AddError(CommonMessage.SystemError);
                return _apiResponse;
            }

            return Ok(_apiResponse);
        }
    }
}
