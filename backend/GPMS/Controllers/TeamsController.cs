using GPMS.DTOS.Team;
using GPMS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(ITeamService teamService, ILogger<TeamsController> logger)
        {
            _teamService = teamService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetAllTeams()
        {
            try
            {
                var teams = await _teamService.GetAllAsync();
                return Ok(teams);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all teams.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{teamId:long}")]
        public async Task<ActionResult<TeamDto?>> GetTeamById(long teamId)
        {
            try
            {
                var team = await _teamService.GetByIdAsync(teamId);
                if (team == null) return NotFound();
                return Ok(team);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting team with ID {TeamId}", teamId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TeamDto>> CreateTeam([FromBody] CreateTeamRequest request)
        {
            try
            {
                if (request.MemberStudentIds == null || !request.MemberStudentIds.Any())
                    return BadRequest("Team must have at least one member (creator).");

                var createdTeam = await _teamService.CreateAsync(request.CreatorStudentId, request.MemberStudentIds, request.TeamName);
                return CreatedAtAction(nameof(GetTeamById), new { teamId = createdTeam.TeamId }, createdTeam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a team for creator {CreatorId}", request.CreatorStudentId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{teamId:long}")]
        public async Task<IActionResult> DeleteTeam(long teamId)
        {
            try
            {
                var success = await _teamService.DeleteAsync(teamId);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting team with ID {TeamId}", teamId);
                return StatusCode(500, "Internal server error");
            }
        }
    }

   
}
