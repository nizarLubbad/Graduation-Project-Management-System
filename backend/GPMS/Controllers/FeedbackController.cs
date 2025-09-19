//using GPMS.DTOS.Feedback;
//using GPMS.Interfaces;
//using Microsoft.AspNetCore.Mvc;

//namespace GPMS.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class FeedbackController : ControllerBase
//    {
//        private readonly IFeedbackService _feedbackService;

//        public FeedbackController(IFeedbackService feedbackService)
//        {
//            _feedbackService = feedbackService;
//        }


//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateFeedbackDto dto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var feedback = await _feedbackService.AddFeedbackAsync(dto);
//            return CreatedAtAction(nameof(GetById), new { id = feedback.Id }, feedback);
//        }


//        [HttpGet("team/{teamId:long}")]
//        public async Task<IActionResult> GetByTeam(long teamId)
//        {
//            var feedbacks = await _feedbackService.GetByTeamIdAsync(teamId);

//            var result = feedbacks.Select(f => new FeedbackResponseDto
//            {
//                Id = f.Id,
//                Content = f.Content,
//                Date = f.Date,
//                SupervisorId = f.SupervisorId ?? 0,
//                SupervisorName = f.Supervisor?.Name,
//                TeamId = f.TeamId,
//                Replies = f.Replies.Select(r => new GPMS.DTOS.Reply.ReplyResponseDto
//                {
//                    Id = r.Id,
//                    Content = r.Content,
//                    Date = r.Date,
//                    StudentId = r.StudentId,
//                    StudentName = r.Student?.Name,
//                    SupervisorId = r.SupervisorId,
//                    SupervisorName = r.Supervisor?.Name
//                }).ToList()
//            });

//            return Ok(result);
//        }


//        [HttpGet("{id:long}")]
//        public async Task<IActionResult> GetById(long id)
//        {
//            var f = await _feedbackService.GetByIdAsync(id);
//            if (f == null) return NotFound();

//            var result = new FeedbackResponseDto
//            {
//                Id = f.Id,
//                Content = f.Content,
//                Date = f.Date,
//                SupervisorId = f.SupervisorId ?? 0,
//                SupervisorName = f.Supervisor?.Name,
//                TeamId = f.TeamId,
//                Replies = f.Replies.Select(r => new GPMS.DTOS.Reply.ReplyResponseDto
//                {
//                    Id = r.Id,
//                    Content = r.Content,
//                    Date = r.Date,
//                    StudentId = r.StudentId,
//                    StudentName = r.Student?.Name,
//                    SupervisorId = r.SupervisorId,
//                    SupervisorName = r.Supervisor?.Name
//                }).ToList()
//            };

//            return Ok(result);
//        }


//        [HttpDelete("{id:long}")]
//        public async Task<IActionResult> Delete(long id)
//        {
//            var deleted = await _feedbackService.DeleteAsync(id);
//            if (!deleted) return NotFound();

//            return NoContent();
//        }
//    }
//}
//using GPMS.DTOS.Feedback;
//using GPMS.Interfaces;
//using Microsoft.AspNetCore.Mvc;

//namespace GPMS.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class FeedbackController : ControllerBase
//    {
//        private readonly IFeedbackService _feedbackService;
//        private readonly ILogger<FeedbackController> _logger;

//        public FeedbackController(IFeedbackService feedbackService, ILogger<FeedbackController> logger)
//        {
//            _feedbackService = feedbackService;
//            _logger = logger;
//        }

//        // GET: api/Feedback/Team/{teamId}
//        [HttpGet("Team/{teamId:long}")]
//        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetFeedbacksByTeamId(long teamId)
//        {
//            try
//            {
//                var feedbacks = await _feedbackService.GetByTeamIdAsync(teamId);
//                return Ok(feedbacks);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while fetching feedbacks for team {TeamId}", teamId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        // GET: api/Feedback/{feedbackId}
//        [HttpGet("{feedbackId:int}")]
//        public async Task<ActionResult<FeedbackDto?>> GetFeedbackById(int feedbackId)
//        {
//            try
//            {
//                var feedback = await _feedbackService.GetByIdAsync(feedbackId);
//                if (feedback == null) return NotFound();
//                return Ok(feedback);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while fetching feedback with ID {FeedbackId}", feedbackId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        // POST: api/Feedback
//        [HttpPost]
//        public async Task<ActionResult<FeedbackDto>> CreateFeedback([FromBody] CreateFeedbackDto dto)
//        {
//            try
//            {
//                var created = await _feedbackService.CreateFeedbackAsync(dto);
//                return CreatedAtAction(nameof(GetFeedbackById), new { feedbackId = created.FeedbackId }, created);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while creating feedback for team {TeamId}", dto.TeamId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        // PUT: api/Feedback/{feedbackId}
//        [HttpPut("{feedbackId:int}")]
//        public async Task<ActionResult<FeedbackDto?>> UpdateFeedback(int feedbackId, [FromBody] UpdateFeedbackDto dto)
//        {
//            try
//            {
//                var updated = await _feedbackService.UpdateFeedbackAsync(feedbackId, dto);
//                if (updated == null) return NotFound();
//                return Ok(updated);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while updating feedback with ID {FeedbackId}", feedbackId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        // DELETE: api/Feedback/{feedbackId}
//        [HttpDelete("{feedbackId:int}")]
//        public async Task<IActionResult> DeleteFeedback(int feedbackId)
//        {
//            try
//            {
//                var success = await _feedbackService.DeleteFeedbackAsync(feedbackId);
//                if (!success) return NotFound();
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while deleting feedback with ID {FeedbackId}", feedbackId);
//                return StatusCode(500, "Internal server error");
//            }
//        }
//    }
//}
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
                return CreatedAtAction(nameof(GetFeedbackById), new { feedbackId = created.FeedbackId }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating feedback for team {TeamId}", dto.TeamId);
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
