//using Microsoft.AspNetCore.Mvc;
//using GPMS.Interfaces;
//using GPMS.DTOS.Reply;

//namespace GPMS.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ReplyController : ControllerBase
//    {
//        private readonly IReplyService _replyService;

//        public ReplyController(IReplyService replyService)
//        {
//            _replyService = replyService;
//        }

//        // POST: api/reply
//        [HttpPost]
//        public async Task<IActionResult> AddReply([FromBody] CreateReplyDto dto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var reply = await _replyService.AddReplyAsync(dto);

//            var response = new ReplyResponseDto
//            {
//                Id = reply.Id,
//                Content = reply.Content,
//                Date = reply.Date,
//                StudentId = reply.StudentId,
//                StudentName = reply.Student?.Name,
//                SupervisorId = reply.SupervisorId,
//                SupervisorName = reply.Supervisor?.Name
//            };

//            return Ok(response);
//        }

//        // GET: api/reply/feedback/{feedbackId}
//        [HttpGet("feedback/{feedbackId}")]
//        public async Task<IActionResult> GetRepliesByFeedbackId(long feedbackId)
//        {
//            var replies = await _replyService.GetByFeedbackIdAsync(feedbackId);

//            var response = replies.Select(r => new ReplyResponseDto
//            {
//                Id = r.Id,
//                Content = r.Content,
//                Date = r.Date,
//                StudentId = r.StudentId,
//                StudentName = r.Student?.Name,
//                SupervisorId = r.SupervisorId,
//                SupervisorName = r.Supervisor?.Name
//            }).ToList();

//            return Ok(response);
//        }
//    }
//}
using GPMS.DTOS.Reply;
using GPMS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReplyController : ControllerBase
    {
        private readonly IReplyService _replyService;
        private readonly ILogger<ReplyController> _logger;

        public ReplyController(IReplyService replyService, ILogger<ReplyController> logger)
        {
            _replyService = replyService;
            _logger = logger;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var reply = await _replyService.GetByIdAsync(id);
                if (reply == null) return NotFound();
                return Ok(reply);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reply with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("feedback/{feedbackId:long}")]
        public async Task<IActionResult> GetByFeedbackId(long feedbackId)
        {
            try
            {
                var replies = await _replyService.GetByFeedbackIdAsync(feedbackId);
                return Ok(replies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching replies for feedback {FeedbackId}", feedbackId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReplyDto dto)
        {
            try
            {
                var created = await _replyService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reply");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateReplyDto dto)
        {
            try
            {
                var updated = await _replyService.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating reply with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _replyService.DeleteAsync(id);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting reply with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

