using GPMS.DTOS.Student;
using GPMS.DTOS.Team;
using GPMS.Helpers;
using GPMS.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TeamDto>>> GetAllTeams()
        //{
        //    try
        //    {
        //        var teams = await _teamService.GetAllAsync();
        //        return Ok(teams);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while getting all teams.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpPost("create")]
        //public async Task<ActionResult<TeamDto>> CreateTeam([FromBody] CreateTeamRequest request)
        //{
        //    try
        //    {
        //        var team = await _teamService.CreateTeamAsync(
        //            request.TeamName,
        //            request.MemberStudentIds
        //        );
        //        _logger.LogInformation($"Team created successfully with ID: {team.TeamId}");
        //        return Ok(team);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error creating team");
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}
        [HttpPost("create")]
        public async Task<ActionResult<TeamDto>> CreateTeam([FromBody] CreateTeamRequest request)
        {
            try
            {
                var team = await _teamService.CreateTeamAsync(request.TeamName, request.MemberStudentIds);

                if (team == null)
                    return BadRequest(new { message = "No team was created. Possible reason: some students already have a team." });

                _logger.LogInformation($"Team created successfully with ID: {team.TeamId}");
                return Ok(team);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating team");
                return StatusCode(500, new { message = "Internal server error" });
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
        [HttpPatch("{teamId}/update-student-status")]
        public async Task<IActionResult> UpdateStudentsStatus(long teamId)
        {
            try
            {
                await _teamService.UpdateStudentsStatusAsync(teamId);
                return Ok(new { message = "Student statuses updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating student status for team ID {TeamId}.", teamId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        //[HttpPost]
        //[ProducesResponseType(typeof(TeamDto), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<TeamDto>> CreateTeam([FromBody] CreateTeamRequest request)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var teamDto = await _teamService.CreateTeamAsync(request.TeamName, request.MemberStudentIds);

        //        return Ok(teamDto);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new { message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        // يمكنك تسجيل الخطأ هنا
        //        return StatusCode(500, new { message = "حدث خطأ داخلي في الخادم" });
        //    }
        //}
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
        [HttpGet("members")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetTeamMembers()
        {
            try
            {
                // This is the CRITICAL change: look for the "sub" claim
                var studentIdClaim = User.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(studentIdClaim))
                {
                    _logger.LogWarning("Unauthorized attempt to get team members: no studentId in token.");
                    return Unauthorized();
                }

                // Make sure to use long.Parse() as we're expecting a long ID
                long studentId = long.Parse(studentIdClaim);

                _logger.LogInformation("Student {StudentId} requested team members.", studentId);
                var members = await _teamService.GetTeamMembersByStudentIdAsync(studentId);
                _logger.LogInformation("Returning {Count} team members for student {StudentId}.", members.Count, studentId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving team members.");
                return StatusCode(500, "Internal server error");
            }
        }
    }

   
}
