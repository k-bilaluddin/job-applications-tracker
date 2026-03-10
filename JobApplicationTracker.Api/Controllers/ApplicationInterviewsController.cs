using JobApplicationTracker.Api.Interfaces;
using JobApplicationTracker.Api.Models.Request;
using JobApplicationTracker.Api.Errors;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Api.Controllers
{
    [ApiController]
    [Route("api/job-applications")]
    public class ApplicationInterviewsController : ControllerBase
    {
        private readonly IApplicationHandler _handler;

        public ApplicationInterviewsController(IApplicationHandler handler)
        {
            _handler = handler;
        }

        [HttpGet("{id}/interviews")]
        public async Task<IActionResult> GetInterviews(string id, CancellationToken cancellationToken)
        {
            var response = await _handler.GetInterviewsAsync(id, cancellationToken);

            if (response is null)
            {
                return NotFound(ErrorCache.NotFound);
            }

            return Ok(response);
        }

        [HttpPost("{id}/interviews")]
        public async Task<IActionResult> AddInterview(string id, [FromBody] AddInterviewRoundRequest request, CancellationToken cancellationToken)
        {
            var response = await _handler.AddInterviewAsync(id, request, cancellationToken);

            if (response is null)
            {
                return NotFound(ErrorCache.NotFound);
            }

            return CreatedAtAction(nameof(GetInterviewById), new { id = id, interviewId = response.Id }, response);
        }

        // Helper route for CreatedAtAction
        [HttpGet("{id}/interviews/{interviewId}")]
        public async Task<IActionResult> GetInterviewById(string id, string interviewId, CancellationToken cancellationToken)
        {
            var interviews = await _handler.GetInterviewsAsync(id, cancellationToken);

            if (interviews is null)
            {
                return NotFound(ErrorCache.NotFound);
            }

            var interview = interviews.Find(x => x.Id == interviewId);
            if (interview is null)
            {
                return NotFound(ErrorCache.NotFound);
            }

            return Ok(interview);
        }

        [HttpPut("{id}/interviews/{interviewId}")]
        public async Task<IActionResult> UpdateInterview(string id, string interviewId, [FromBody] UpdateInterviewRoundRequest request, CancellationToken cancellationToken)
        {
            var response = await _handler.UpdateInterviewAsync(id, interviewId, request, cancellationToken);

            if (response is null)
            {
                return NotFound(ErrorCache.NotFound);
            }

            return Ok(response);
        }

        [HttpDelete("{id}/interviews/{interviewId}")]
        public async Task<IActionResult> DeleteInterview(string id, string interviewId, CancellationToken cancellationToken)
        {
            var deleted = await _handler.DeleteInterviewAsync(id, interviewId, cancellationToken);

            if (!deleted)
            {
                return NotFound(ErrorCache.NotFound);
            }

            return NoContent();
        }
    }
}
