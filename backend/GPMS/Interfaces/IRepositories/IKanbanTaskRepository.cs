using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface ITaskRepository : IBaseRepository<KanbanTask, long>
    {
        Task<IEnumerable<KanbanTask>> GetByTeamIdAsync(long teamId);
    }
}
