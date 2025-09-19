//using GPMS.DTOS.Link;
//using GPMS.Interfaces;
//using Microsoft.AspNetCore.Mvc;

//namespace GPMS.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class LinkController : ControllerBase
//    {
//        private readonly ILinkService _linkService;

//        public LinkController(ILinkService linkService)
//        {
//            _linkService = linkService;
//        }

//        // GET: api/Link/team/5
//        [HttpGet("team/{teamId}")]
//        public async Task<IActionResult> GetByTeam(long teamId)
//        {
//            var links = await _linkService.GetByTeamIdAsync(teamId);

//            var result = links.Select(l => new LinkDto
//            {
//                Id = l.Id,
//                Url = l.Url,
//                Title = l.Title,
//                Date = l.Date,
//                StudentId = l.StudentId,
//                StudentName = l.Student.Name, // نفترض عندك Student.Name
//                TeamId = l.TeamId
//            });

//            return Ok(result);
//        }

//        // GET: api/Link/5
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(long id)
//        {
//            var link = await _linkService.GetByIdAsync(id);
//            if (link == null) return NotFound();

//            var result = new LinkDto
//            {
//                Id = link.Id,
//                Url = link.Url,
//                Title = link.Title,
//                Date = link.Date,
//                StudentId = link.StudentId,
//                StudentName = link.Student.Name,
//                TeamId = link.TeamId
//            };

//            return Ok(result);
//        }

//        // POST: api/Link
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateLinkDto dto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var link = await _linkService.AddLinkAsync(dto);

//            return CreatedAtAction(nameof(GetById), new { id = link.Id }, link);
//        }

//        // DELETE: api/Link/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(long id)
//        {
//            var deleted = await _linkService.DeleteAsync(id);
//            if (!deleted) return NotFound();

//            return NoContent();
//        }
//    }
//}
using GPMS.DTOS.Link;
using GPMS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LinkController : ControllerBase
    {
        private readonly ILinkService _linkService;
        private readonly ILogger<LinkController> _logger;

        public LinkController(ILinkService linkService, ILogger<LinkController> logger)
        {
            _linkService = linkService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var links = await _linkService.GetAllAsync();
                return Ok(links);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all links");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var link = await _linkService.GetByIdAsync(id);
                if (link == null) return NotFound();
                return Ok(link);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving link with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLinkDto dto)
        {
            try
            {
                var created = await _linkService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating link");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] LinkDto dto)
        {
            try
            {
                var updated = await _linkService.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating link with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var deleted = await _linkService.DeleteAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting link with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("team/{teamId:long}")]
        public async Task<IActionResult> GetByTeamId(long teamId)
        {
            try
            {
                var links = await _linkService.GetByTeamIdAsync(teamId);
                return Ok(links);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving links for Team ID {TeamId}", teamId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("student/{studentId:long}")]
        public async Task<IActionResult> GetByStudentId(long studentId)
        {
            try
            {
                var links = await _linkService.GetByStudentIdAsync(studentId);
                return Ok(links);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving links for Student ID {StudentId}", studentId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}


