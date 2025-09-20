using GPMS.DTOS.Auth;
using GPMS.Interfaces;
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
    }
}
