using EventHub.Contracts;
using EventHub.Dtos.EventUploads;
using EventHub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventUploadController : ControllerBase
    {
        private readonly IEventUploadService _eventUploadService;

        public EventUploadController(IEventUploadService eventUploadService)
        {
            _eventUploadService = eventUploadService;
        }

       

        [HttpPost("create-eventupload")]
        public async Task<IActionResult> Create([FromForm] EventUploadRequest request)
        {
            var response = await _eventUploadService.CreateUploadAsync(request);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("get-eventupload/{id}")]
        public async Task<IActionResult> GetRecordById(Guid id)
        {
            var response = await _eventUploadService.GetRecordByIdAsync(id);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("get-all-eventuploads")]
        public async Task<IActionResult> GetAllRecord()
        {
            var response = await _eventUploadService.GetAllRecordAsync();

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut("update-eventupload")]
        public async Task<IActionResult> Update([FromForm] Guid Id, UpdateEventUploadRequest request)
        {
            var response = await _eventUploadService.UpdateUploadAsync(Id, request);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpDelete("delete-eventupload/{id}")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var response = await _eventUploadService.DeleteUploadAsync(id);

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

