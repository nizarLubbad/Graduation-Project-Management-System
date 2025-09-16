using Microsoft.AspNetCore.Mvc;
using GPMS.Interfaces;
using GPMS.DTOS.Reply;

namespace GPMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReplyController : ControllerBase
    {
        private readonly IReplyService _replyService;

        public ReplyController(IReplyService replyService)
        {
            _replyService = replyService;
        }

        // POST: api/reply
        [HttpPost]
        public async Task<IActionResult> AddReply([FromBody] CreateReplyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reply = await _replyService.AddReplyAsync(dto);

            var response = new ReplyResponseDto
            {
                Id = reply.Id,
                Content = reply.Content,
                Date = reply.Date,
                StudentId = reply.StudentId,
                StudentName = reply.Student?.Name,
                SupervisorId = reply.SupervisorId,
                SupervisorName = reply.Supervisor?.Name
            };

            return Ok(response);
        }

        // GET: api/reply/feedback/{feedbackId}
        [HttpGet("feedback/{feedbackId}")]
        public async Task<IActionResult> GetRepliesByFeedbackId(long feedbackId)
        {
            var replies = await _replyService.GetByFeedbackIdAsync(feedbackId);

            var response = replies.Select(r => new ReplyResponseDto
            {
                Id = r.Id,
                Content = r.Content,
                Date = r.Date,
                StudentId = r.StudentId,
                StudentName = r.Student?.Name,
                SupervisorId = r.SupervisorId,
                SupervisorName = r.Supervisor?.Name
            }).ToList();

            return Ok(response);
        }
    }
}
