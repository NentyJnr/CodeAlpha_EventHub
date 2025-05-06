using EventHub.Contracts;
using EventHub.Dtos.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("create-event")]
        public async Task<IActionResult> Create([FromForm] CreateEventRequest request)
        {
            var response = await _eventService.CreateAsync(request);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("get-events/{id}")]
        public async Task<IActionResult> GetRecordById(Guid id)
        {
            var response = await _eventService.GetRecordById(id);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("get-all-events")]
        public async Task<IActionResult> GetAllRecord()
        {
            var response = await _eventService.GetAllRecord();

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
        public async Task<IActionResult> Update([FromForm] UpdateEventRequest request)
        {
            var response = await _eventService.UpdateAsync(request);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpDelete("deactivate-event/{id}")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var response = await _eventService.DeactivateAsync(id);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("activate-event/{id}")]
        public async Task<IActionResult> Activate(Guid id)
        {
            var response = await _eventService.ActivateAsync(id);

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
