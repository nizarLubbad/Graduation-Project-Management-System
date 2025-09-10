using Microsoft.AspNetCore.Mvc;

namespace YourProjectName.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // GET: api/user
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(new { message = "Users list retrieved successfully" });
        }

        // POST: api/user
        [HttpPost]
        public IActionResult CreateUser()
        {
            return Ok(new { message = "User created successfully" });
        }
    }
}
