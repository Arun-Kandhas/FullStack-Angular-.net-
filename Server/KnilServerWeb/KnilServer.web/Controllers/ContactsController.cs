using KnilServer.Application.ApplicationConstant;
using KnilServer.Application.Common;
using KnilServer.Application.DTO.Contacts;
using KnilServer.Application.Services.Interfaces;
using KnilServer.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KnilServer.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactServices _contactServices;
        private readonly ILogger<ContactsController> _logger;
        protected APIResponse _apiResponse;

        public ContactsController(IContactServices contactServices,ILogger<ContactsController> logger)
        {
            _contactServices = contactServices;
            _apiResponse = new APIResponse();
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> GetContacts()
        {
            try
            {
                var Contacts = await _contactServices.GetAllAsync();
                if (Contacts == null || Contacts.Count == 0)
                {
                    _apiResponse.StatusCode =System.Net.HttpStatusCode.NoContent;
                    _apiResponse.IsSuccess = false;                   
                    _apiResponse.DisplayMessage = CommonMessage.RecordNotFound;
                    return _apiResponse;
                }

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = Contacts;
                _apiResponse.DisplayMessage = "Success";
                _logger.LogInformation("Record Fetched");


            }
            catch (Exception)
            {
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.AddError(CommonMessage.SystemError);
                return _apiResponse;
            }

            return _apiResponse;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> GetContactById(int id)
        {
            try
            {
                var Contact = await _contactServices.GetByIdAsync(id);
                if (Contact == null)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.DisplayMessage = CommonMessage.RecordNotFound;
                    return _apiResponse;
                }

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = Contact;
                _apiResponse.DisplayMessage = "Success";
                _logger.LogInformation("Record Fetched");

            }
            catch (Exception)
            {
                _logger.LogError("Contact controller get Inoformation Failed");
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.AddError(CommonMessage.SystemError);
                return _apiResponse;
            }
            return _apiResponse;
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<APIResponse>> CreateContact([FromBody] CreateContactDTO contactDto)
        {
            try
            {   
                if(!ModelState.IsValid)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.AddError(ModelState.ToString()!);
                    return _apiResponse;
                }
                

                var result = await _contactServices.CreateAsync(contactDto);

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
            return _apiResponse;
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateContact(int id, [FromBody] UpdateContactDto contactdto)
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

                if (contactdto == null || id != contactdto.Id)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.AddError("Id is not matched");
                    return _apiResponse;
                }
                
               var result =  await _contactServices.UpdateAsync(contactdto!);
                if (result == null)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.DisplayMessage = CommonMessage.RecordNotFound;
                    return _apiResponse;
                }
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
            return _apiResponse;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteContact(int id)
        {
            try
            {
                var Contact = await _contactServices.GetByIdAsync(id);

                if (Contact == null)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
                    _apiResponse.IsSuccess = false;
                    _apiResponse.DisplayMessage = CommonMessage.RecordNotFound;
                    return _apiResponse;
                }
                await _contactServices.DeleteAsync(id);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;                
                _apiResponse.DisplayMessage = "Success";
            }
            catch (Exception)
            {
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.AddError(CommonMessage.SystemError);
                return _apiResponse;

            }
            return _apiResponse;
        }
    }
}
