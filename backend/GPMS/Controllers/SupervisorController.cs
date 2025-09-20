using GPMS.DTOS;
using GPMS.DTOS.Project;
using GPMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace GPMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisorController : ControllerBase
    {
        private readonly SupervisorService _supervisorService;

        public SupervisorController(SupervisorService supervisorService)
        {
            _supervisorService = supervisorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupervisorDto>>> GetAll()
        {
            var supervisors = await _supervisorService.GetAllAsync();
            return Ok(supervisors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupervisorDto>> GetById(int id)
        {
            var supervisor = await _supervisorService.GetByIdAsync(id);
            if (supervisor == null) return NotFound();
            return Ok(supervisor);
        }
    }
}
