namespace GPMS.Interfaces
{
    public interface IKanbanTaskService : IKanbanTaskRepository
    {
        // modify task status to be todo,doing,done
        Task<bool> ModifyTaskStatusAsync(int taskId, string status);
    }
}
