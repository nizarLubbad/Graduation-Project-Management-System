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

        public LinkController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        // GET: api/Link/team/5
        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetByTeam(long teamId)
        {
            var links = await _linkService.GetByTeamIdAsync(teamId);

            var result = links.Select(l => new LinkDto
            {
                Id = l.Id,
                Url = l.Url,
                Title = l.Title,
                Date = l.Date,
                StudentId = l.StudentId,
                StudentName = l.Student.Name, // نفترض عندك Student.Name
                TeamId = l.TeamId
            });

            return Ok(result);
        }

        // GET: api/Link/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var link = await _linkService.GetByIdAsync(id);
            if (link == null) return NotFound();

            var result = new LinkDto
            {
                Id = link.Id,
                Url = link.Url,
                Title = link.Title,
                Date = link.Date,
                StudentId = link.StudentId,
                StudentName = link.Student.Name,
                TeamId = link.TeamId
            };

            return Ok(result);
        }

        // POST: api/Link
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLinkDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var link = await _linkService.AddLinkAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = link.Id }, link);
        }

        // DELETE: api/Link/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _linkService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
