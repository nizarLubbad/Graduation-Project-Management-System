//using GPMS.DTOS.Task;
//using GPMS.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using TaskStatus = GPMS.Models.Enums.TaskStatus;
//using TaskPriority = GPMS.Models.Enums.TaskPriority;
//namespace GPMS.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class KanbanTaskController : ControllerBase
//    {
//        private readonly IKanbanTaskService _taskService;

//        public KanbanTaskController(IKanbanTaskService taskService)
//        {
//            _taskService = taskService;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var task = await _taskService.CreateAsync(dto);
//            return Ok(task);
//        }


//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(long id, [FromBody] UpdateTaskDto dto)
//        {
//            var task = await _taskService.UpdateAsync(id, dto);
//            if (task == null) return NotFound("Task not found.");

//            return Ok(task);
//        }


//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(long id)
//        {
//            var task = await _taskService.GetByIdAsync(id);
//            if (task == null) return NotFound("Task not found.");

//            return Ok(task);
//        }


//        [HttpGet("team/{teamId}")]
//        public async Task<IActionResult> GetByTeamId(long teamId)
//        {
//            var tasks = await _taskService.GetByTeamIdAsync(teamId);
//            return Ok(tasks);
//        }


//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(long id)
//        {
//            var deleted = await _taskService.DeleteAsync(id);
//            if (!deleted) return NotFound("Task not found.");

//            return Ok("Task deleted successfully.");
//        }


//        [HttpPatch("{id}/status")]
//        public async Task<IActionResult> ChangeStatus(long id, [FromQuery] TaskStatus newStatus)
//        {
//            var task = await _taskService.ChangeStatusAsync(id, newStatus);
//            if (task == null) return NotFound("Task not found.");

//            return Ok(task);
//        }


//        [HttpPatch("{id}/priority")]
//        public async Task<IActionResult> ChangePriority(long id, [FromQuery] TaskPriority newPriority)
//        {
//            var task = await _taskService.ChangePriorityAsync(id, newPriority);
//            if (task == null) return NotFound("Task not found.");

//            return Ok(task);
//        }


//        [HttpPatch("{id}/assigned-students")]
//        public async Task<IActionResult> UpdateAssignedStudents(long id, [FromBody] List<long> assignedStudentIds)
//        {
//            var task = await _taskService.UpdateAssignedStudentsAsync(id, assignedStudentIds);
//            if (task == null) return NotFound("Task not found.");

//            return Ok(task);
//        }
//    }
//}
using GPMS.DTOS.Task;
using GPMS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KanbanTaskController : ControllerBase
    {
        private readonly IKanbanTaskService _taskService;
        private readonly ILogger<KanbanTaskController> _logger;

        public KanbanTaskController(IKanbanTaskService taskService, ILogger<KanbanTaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }
        [HttpGet("team/{teamId:long}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetByTeam(long teamId)
        {
            try
            {
                var tasks = await _taskService.GetAllByTeamIdAsync(teamId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tasks for team {TeamId}", teamId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
        {
            try
            {
                var tasks = await _taskService.GetAllAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all tasks");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{taskId:long}")]
        public async Task<ActionResult<TaskDto?>> GetById(long taskId)
        {
            try
            {
                var task = await _taskService.GetByIdAsync(taskId);
                if (task == null) return NotFound();
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching task with ID {TaskId}", taskId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto dto)
        {
            try
            {
                var created = await _taskService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { taskId = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task for team {TeamId}", dto.TeamId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{taskId:long}")]
        public async Task<ActionResult<TaskDto?>> Update(long taskId, [FromBody] UpdateTaskDto dto)
        {
            try
            {
                var updated = await _taskService.UpdateAsync(taskId, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task with ID {TaskId}", taskId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("status")]
        public async Task<ActionResult<TaskDto?>> UpdateStatus([FromBody] UpdateTaskStatusDto dto)
        {
            try
            {
                var updated = await _taskService.UpdateStatusAsync(dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for task ID {TaskId}", dto.TaskId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{taskId:long}")]
        public async Task<IActionResult> Delete(long taskId)
        {
            try
            {
                var success = await _taskService.DeleteAsync(taskId);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task with ID {TaskId}", taskId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

