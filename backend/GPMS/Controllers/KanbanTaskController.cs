using GPMS.DTOS.Task;
using GPMS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskStatus = GPMS.Models.Enums.TaskStatus;
using TaskPriority = GPMS.Models.Enums.TaskPriority;
namespace GPMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KanbanTaskController : ControllerBase
    {
        private readonly IKanbanTaskService _taskService;

        public KanbanTaskController(IKanbanTaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var task = await _taskService.CreateAsync(dto);
            return Ok(task);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateTaskDto dto)
        {
            var task = await _taskService.UpdateAsync(id, dto);
            if (task == null) return NotFound("Task not found.");

            return Ok(task);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound("Task not found.");

            return Ok(task);
        }

       
        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetByTeamId(long teamId)
        {
            var tasks = await _taskService.GetByTeamIdAsync(teamId);
            return Ok(tasks);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _taskService.DeleteAsync(id);
            if (!deleted) return NotFound("Task not found.");

            return Ok("Task deleted successfully.");
        }

       
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(long id, [FromQuery] TaskStatus newStatus)
        {
            var task = await _taskService.ChangeStatusAsync(id, newStatus);
            if (task == null) return NotFound("Task not found.");

            return Ok(task);
        }

       
        [HttpPatch("{id}/priority")]
        public async Task<IActionResult> ChangePriority(long id, [FromQuery] TaskPriority newPriority)
        {
            var task = await _taskService.ChangePriorityAsync(id, newPriority);
            if (task == null) return NotFound("Task not found.");

            return Ok(task);
        }

        
        [HttpPatch("{id}/assigned-students")]
        public async Task<IActionResult> UpdateAssignedStudents(long id, [FromBody] List<long> assignedStudentIds)
        {
            var task = await _taskService.UpdateAssignedStudentsAsync(id, assignedStudentIds);
            if (task == null) return NotFound("Task not found.");

            return Ok(task);
        }
    }
}
