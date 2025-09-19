
using GPMS.DTOS.Task;
using GPMS.Models;

using TaskPriorityEnum = GPMS.Models.Enums.TaskPriority;
using TaskStatusEnum = GPMS.Models.Enums.TaskStatus;

namespace GPMS.Interfaces
{
    public interface IKanbanTaskService
    {
        //Task<KanbanTask> CreateAsync(CreateTaskDto dto);
        //Task<KanbanTask?> UpdateAsync(long id, UpdateTaskDto dto);
        //Task<KanbanTask?> GetByIdAsync(long id);
        //Task<IEnumerable<KanbanTask>> GetByTeamIdAsync(long teamId);
        //Task<bool> DeleteAsync(long id);
        //Task<KanbanTask?> ChangeStatusAsync(long id, TaskStatusEnum newStatus);
        //Task<KanbanTask?> UpdateAssignedStudentsAsync(long id, List<long> assignedStudentIds);
        //Task<KanbanTask?> ChangePriorityAsync(long id, TaskPriorityEnum newPriority);
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task<TaskDto?> GetByIdAsync(long taskId);
        Task<IEnumerable<TaskDto>> GetAllAsync();
        Task<TaskDto?> UpdateAsync(long taskId, UpdateTaskDto dto);
        Task<TaskDto?> UpdateStatusAsync(UpdateTaskStatusDto dto);
        Task<bool> DeleteAsync(long taskId);
    }
}
