
using GPMS.DTOS.Supervisor;
using GPMS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GPMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Supervisor")]
    public class SupervisorsController : ControllerBase
    {
        private readonly ISupervisorService _supervisorService;
        private readonly ILogger<SupervisorsController> _logger;

        public SupervisorsController(ISupervisorService supervisorService, ILogger<SupervisorsController> logger)
        {
            _supervisorService = supervisorService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllSupervisors()
        {
            try
            {
                var supervisors = await _supervisorService.GetAllSupervisorsAsync();
                _logger.LogInformation("Retrieved {Count} supervisors successfully.", supervisors.Count);

                return Ok(new
                {
                    success = true,
                    count = supervisors.Count,
                    supervisors
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving supervisors.");
                return BadRequest(new
                {
                    success = false,
                    message = "Error retrieving supervisors: " + ex.Message
                });
            }
        }
        [HttpPost("{userId}/max-teams")]
        public async Task<IActionResult> SetMaxTeams(long userId, [FromBody] int maxTeams)
        {
            var result = await _supervisorService.SetMaxTeamsAsync(userId, maxTeams);
            if (!result)
                return NotFound(new { message = "Supervisor not found" });

            return Ok(new { message = "Max teams updated successfully" });
        }

        [HttpPost("{supervisorId}/book-team/{teamId}")]
        public async Task<IActionResult> BookTeam(long supervisorId, long teamId)
        {
            var (success, errorMessage) = await _supervisorService.BookTeamAsync(supervisorId, teamId);

            if (!success)
                return BadRequest(new { message = errorMessage });

            return Ok(new { message = "Team booked successfully." });
        }
        [HttpPatch("{userId}/increment-team-count")]
        public async Task<IActionResult> IncrementTeamCount(long userId)
        {
            var result = await _supervisorService.IncrementTeamCountAsync(userId);
            if (!result)
                return BadRequest(new { message = "Cannot increment team count (max reached or supervisor not found)" });

            return Ok(new { message = "Team count incremented" });
        }

        [HttpPatch("{userId}/decrement-team-count")]
        public async Task<IActionResult> DecrementTeamCount(long userId)
        {
            var result = await _supervisorService.DecrementTeamCountAsync(userId);
            if (!result)
                return BadRequest(new { message = "Cannot decrement team count (already zero or supervisor not found)" });

            return Ok(new { message = "Team count decremented" });
        }

        [HttpGet("{userId}/remaining-teams")]
        public async Task<IActionResult> GetRemainingTeams(long userId)
        {
            var remaining = await _supervisorService.GetRemainingTeamsAsync(userId);
            if (remaining == null)
                return NotFound(new { message = "Supervisor not found" });

            return Ok(new { remainingTeams = remaining });
        }
        [HttpGet("{userId}/max-teams")]
        public async Task<ActionResult<int?>> GetMaxTeams(long userId)
        {
            try
            {
                var maxTeams = await _supervisorService.GetMaxTeamsAsync(userId);
                if (maxTeams == null)
                {
                    _logger.LogWarning("Supervisor with ID {UserId} not found", userId);
                    return NotFound(new { message = "Supervisor not found" });
                }

                _logger.LogInformation("Supervisor {UserId} MaxTeams: {MaxTeams}", userId, maxTeams);
                return Ok(new { MaxTeams = maxTeams });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting MaxTeams for supervisor {UserId}", userId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


    }

}
