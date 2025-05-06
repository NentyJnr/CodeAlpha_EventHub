using EventHub.Contracts;
using EventHub.Dtos.GuestSpeakers;
using EventHub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestSpeakerController : ControllerBase
    {
        private readonly IGuestSpeakerService _speakerService;

        public GuestSpeakerController(IGuestSpeakerService speakerService)
        {
            _speakerService = speakerService;
        }

        [HttpPost("create-guestspeaker")]
        public async Task<IActionResult> Create([FromForm] GuestSpeakerRequest request)
        {
            var response = await _speakerService.CreateAsync(request);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("get-guestspeaker/{id}")]
        public async Task<IActionResult> GetRecordById(Guid id)
        {
            var response = await _speakerService.GetRecordByIdAsync(id);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("get-all-guestspeaker")]
        public async Task<IActionResult> GetAllRecord()
        {
            var response = await _speakerService.GetAllRecordAsync();

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut("update-guestspeaker")]
        public async Task<IActionResult> Update([FromForm] Guid Id, UpdateGuestSpeakerRequest request)
        {
            var response = await _speakerService.UpdateAsync(Id, request);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpDelete("delete-guestspeaker/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _speakerService.DeleteAsync(id);

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


