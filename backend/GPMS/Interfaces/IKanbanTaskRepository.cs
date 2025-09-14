using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IKanbanTaskRepository : IBaseRepository<KanbanTask>
    {
        Task<string?> GetTaskStatusAsync(int taskId);
        //Task<List<KanbanTask>> GetTasksByTeamIdAsync(long teamId); // in Interface ITaskService
        //Task<bool> UpdateTaskStatusAsync(int taskId, string newStatus); in Interface ITaskService

    }
}