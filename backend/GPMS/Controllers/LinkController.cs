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

        // GET: api/Link/team/{teamId}
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<IEnumerable<LinkDto>>> GetLinksByTeamId(long teamId)
        {
            var links = await _linkService.GetLinksByTeamIdAsync(teamId);

            var linkDtos = links.Select(l => new LinkDto
            {
                Id = l.Id,
                Url = l.Url,
                Title = l.Title,
                Description = l.Description,
                TeamId = l.TeamId
            });

            return Ok(linkDtos);
        }

        // POST: api/Link
        [HttpPost]
        public async Task<ActionResult<LinkDto>> AddLink([FromBody] CreateLinkDto dto)
        {
            var link = await _linkService.AddLinkAsync(dto);

            var linkDto = new LinkDto
            {
                Id = link.Id,
                Url = link.Url,
                Title = link.Title,
                Description = link.Description,
                TeamId = link.TeamId
            };

            return CreatedAtAction(nameof(GetLinksByTeamId), new { teamId = link.TeamId }, linkDto);
        }

        // PUT: api/Link/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<LinkDto>> UpdateLink(int id, [FromBody] LinkDto dto)
        {
            var updatedLink = await _linkService.UpdateLinkAsync(id, dto);
            if (updatedLink == null) return NotFound();

            var linkDto = new LinkDto
            {
                Id = updatedLink.Id,
                Url = updatedLink.Url,
                Title = updatedLink.Title,
                Description = updatedLink.Description,
                TeamId = updatedLink.TeamId
            };

            return Ok(linkDto);
        }

        // DELETE: api/Link/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLink(int id)
        {
            var deleted = await _linkService.DeleteLinkAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
