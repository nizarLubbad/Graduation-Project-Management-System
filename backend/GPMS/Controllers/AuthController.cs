using GPMS.DTOS.Auth;
using GPMS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GPMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register/student")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDto dto)
        {
            try
            {
                await _authService.RegisterStudentAsync(dto);
                return Ok(new { message = "Student registered successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering student with email {Email}", dto.Email);

                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("register/supervisor")]
        public async Task<IActionResult> RegisterSupervisor([FromBody] RegisterSupervisorDto dto)
        {
            try
            {
                await _authService.RegisterSupervisorAsync(dto);
                return Ok(new { message = "Supervisor registered successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering supervisor with email {Email}", dto.Email);

                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var response = await _authService.LoginAsync(dto);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized login attempt with email {Email}", dto.Email);

                return Unauthorized(new { message = "Invalid email or password" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for email {Email}", dto.Email);

                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("getUsers")]

        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _authService.GetAllUsersAsync();

                return Ok(new
                {
                    success = true,
                    message = "Users retrieved successfully",
                    count = users.Count(),
                    users = users
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving users");
                return BadRequest(new
                {
                    success = false,
                    message = "Error retrieving users: " + ex.Message
                });
            }
        }
        [HttpGet("status/{userId:long}")]
        public async Task<ActionResult<bool?>> GetUserStatus(long userId)
        {
            try
            {
                var status = await _authService.GetUserStatusAsync(userId);
                if (status == null)
                {
                    return NotFound("User or student details not found.");
                }

                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving user status.");
            }
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(long userId)
        {
            try
            {
                var user = await _authService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user with ID {UserId}", userId);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("delete-all-users")]
        public async Task<IActionResult> DeleteAllUsers()
        {
            try
            {
                _logger.LogInformation("Admin user requested to delete all users");

                var deletedCount = await _authService.DeleteAllUsersAsync();

                return Ok(new
                {
                    Message = "All users deleted successfully",
                    DeletedRecords = deletedCount,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting all users");
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
