using EventHub.Contracts;
using EventHub.Dtos.Events;
using EventHub.Dtos.Tags;
using EventHub.Entities;
using EventHub.Responses;
using EventHub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

       
        [HttpPut("upload-tag-passport")]
        public async Task<IActionResult> UploadTagPassport([FromForm] UploadTagPassportRequest request)
        {
            var response = await _tagService.UploadPassport(request);

            if (response.Status)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("get-tag")]
        public async Task<IActionResult> GetTag([FromBody] TagRequest request)
        {
            var response = await _tagService.GetTag(request);
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


