using GPMS.Models;
using GPMS.DTOS.Task;
using TaskStatusEnum = GPMS.Models.Enums.TaskStatus;
using TaskPriorityEnum = GPMS.Models.Enums.TaskPriority;

namespace GPMS.Interfaces
{
    public interface IKanbanTaskService
    {
        Task<KanbanTask> CreateAsync(CreateTaskDto dto);
        Task<KanbanTask?> UpdateAsync(long id, UpdateTaskDto dto);
        Task<KanbanTask?> GetByIdAsync(long id);
        Task<IEnumerable<KanbanTask>> GetByTeamIdAsync(long teamId);
        Task<bool> DeleteAsync(long id);
        Task<KanbanTask?> ChangeStatusAsync(long id, TaskStatusEnum newStatus);
        Task<KanbanTask?> UpdateAssignedStudentsAsync(long id, List<long> assignedStudentIds);
        Task<KanbanTask?> ChangePriorityAsync(long id, TaskPriorityEnum newPriority);
    }
}
