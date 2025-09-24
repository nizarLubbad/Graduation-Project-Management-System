using GPMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GPMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenTestController : ControllerBase
    {
        private readonly TokenTestService _tokenService;

        public TokenTestController(TokenTestService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("generate")]
        public IActionResult GenerateToken([FromQuery] long studentId = 98)
        {
            var token = _tokenService.GenerateTestToken(studentId);
            return Ok(new { token });
        }
    }
}
