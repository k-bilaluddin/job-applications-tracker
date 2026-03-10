using JobApplicationTracker.Api.Interfaces;
using JobApplicationTracker.Api.Models.Request;
using Microsoft.AspNetCore.Mvc;
using JobApplicationTracker.Api.Errors;

namespace JobApplicationTracker.Api.Controllers
{
    [ApiController]
    [Route("api/job-applications")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationHandler _handler;

        public ApplicationController(IApplicationHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobApplicationRequest request, CancellationToken cancellationToken)
        {
            var response = await _handler.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            var response = await _handler.GetByIdAsync(id, cancellationToken);

            if (response is null)
            {
                return NotFound(ErrorCache.NotFound);
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await _handler.GetAllAsync(cancellationToken);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateJobApplicationRequest request, CancellationToken cancellationToken)
        {
            var response = await _handler.UpdateAsync(id, request, cancellationToken);

            if (response is null)
            {
                return NotFound(ErrorCache.NotFound);
            }

            return Ok(response);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateApplicationStatusRequest request, CancellationToken cancellationToken)
        {
            var response = await _handler.UpdateStatusAsync(id, request, cancellationToken);

            if (response is null)
            {
                return NotFound(ErrorCache.NotFound);
            }

            return Ok(response);
        }
    }
}
