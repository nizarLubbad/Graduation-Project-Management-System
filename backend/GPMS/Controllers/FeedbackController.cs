
using GPMS.DTOS.Feedback;
using GPMS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(IFeedbackService feedbackService, ILogger<FeedbackController> logger)
        {
            _feedbackService = feedbackService;
            _logger = logger;
        }

        [HttpGet("Team/{teamId:long}")]
        public async Task<ActionResult<IEnumerable<FeedbackResponseDto>>> GetFeedbacksByTeamId(long teamId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetByTeamIdAsync(teamId);
                if (feedbacks == null || !feedbacks.Any())
                    return NotFound();

                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching feedbacks for team {TeamId}", teamId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{feedbackId:long}")]
        public async Task<ActionResult<FeedbackDto?>> GetFeedbackById(long feedbackId)
        {
            try
            {
                var feedback = await _feedbackService.GetByIdAsync(feedbackId);
                if (feedback == null) return NotFound();

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching feedback with ID {FeedbackId}", feedbackId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FeedbackDto>> CreateFeedback([FromBody] CreateFeedbackDto dto)
        {
            try
            {
                var created = await _feedbackService.CreateFeedbackAsync(dto);
                _logger.LogInformation($"Feedback created by Supervisor {dto.SupervisorId} for Team {dto.TeamId}");
                return CreatedAtAction(nameof(GetFeedbackById), new { feedbackId = created.FeedbackId }, created);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Supervisor {SupervisorId} tried to add feedback to Team {TeamId} without permission", dto.SupervisorId, dto.TeamId);
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Team {TeamId} not found for feedback creation", dto.TeamId);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating feedback for Team {TeamId}", dto.TeamId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{feedbackId:long}")]
        public async Task<ActionResult<FeedbackDto?>> UpdateFeedback(long feedbackId, [FromBody] UpdateFeedbackDto dto)
        {
            try
            {
                var updated = await _feedbackService.UpdateFeedbackAsync(feedbackId, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating feedback with ID {FeedbackId}", feedbackId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{feedbackId:long}")]
        public async Task<IActionResult> DeleteFeedback(long feedbackId)
        {
            try
            {
                var success = await _feedbackService.DeleteFeedbackAsync(feedbackId);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting feedback with ID {FeedbackId}", feedbackId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
