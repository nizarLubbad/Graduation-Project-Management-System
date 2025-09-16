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

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFeedbackDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var feedback = await _feedbackService.AddFeedbackAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = feedback.Id }, feedback);
        }

        
        [HttpGet("team/{teamId:long}")]
        public async Task<IActionResult> GetByTeam(long teamId)
        {
            var feedbacks = await _feedbackService.GetByTeamIdAsync(teamId);

            var result = feedbacks.Select(f => new FeedbackResponseDto
            {
                Id = f.Id,
                Content = f.Content,
                Date = f.Date,
                SupervisorId = f.SupervisorId ?? 0,
                SupervisorName = f.Supervisor?.Name,
                TeamId = f.TeamId,
                Replies = f.Replies.Select(r => new GPMS.DTOS.Reply.ReplyResponseDto
                {
                    Id = r.Id,
                    Content = r.Content,
                    Date = r.Date,
                    StudentId = r.StudentId,
                    StudentName = r.Student?.Name,
                    SupervisorId = r.SupervisorId,
                    SupervisorName = r.Supervisor?.Name
                }).ToList()
            });

            return Ok(result);
        }

       
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var f = await _feedbackService.GetByIdAsync(id);
            if (f == null) return NotFound();

            var result = new FeedbackResponseDto
            {
                Id = f.Id,
                Content = f.Content,
                Date = f.Date,
                SupervisorId = f.SupervisorId ?? 0,
                SupervisorName = f.Supervisor?.Name,
                TeamId = f.TeamId,
                Replies = f.Replies.Select(r => new GPMS.DTOS.Reply.ReplyResponseDto
                {
                    Id = r.Id,
                    Content = r.Content,
                    Date = r.Date,
                    StudentId = r.StudentId,
                    StudentName = r.Student?.Name,
                    SupervisorId = r.SupervisorId,
                    SupervisorName = r.Supervisor?.Name
                }).ToList()
            };

            return Ok(result);
        }

        
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _feedbackService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
