using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface ITaskRepository : IBaseRepository<KanbanTask>
    {
        Task<IEnumerable<KanbanTask>> GetByTeamIdAsync(long teamId);
    }
}
