using EventHub.Contracts;
using EventHub.Dtos.Events;
using EventHub.Dtos.Registrations;
using EventHub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _regService;

        public RegistrationController(IRegistrationService regService)
        {
            _regService = regService;
        }


        [HttpPost("register-event")]
        public async Task<IActionResult> Create([FromForm] RegisterRequest request)
        {
            var response = await _regService.RegisterAsync(request);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("get-registration/{id}")]
        public async Task<IActionResult> GetRecordById(Guid registrationId)
        {
            var response = await _regService.GetRegistrationByIdAsync(registrationId);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("get-all-registration-by-event")]
        public async Task<IActionResult> GetAllRecord(Guid eventId)
        {
            var response = await _regService.GetAllRegistrationByEventAsync(eventId);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut("update-event")]
        public async Task<IActionResult> Update([FromForm] Guid registrationId, UpdateRegisterRequest request)
        {
            var response = await _regService.UpdateRegistrationAsync(registrationId, request);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}


