using GPMS.Models;
using GPMS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class TaskRepository : BaseRepository<KanbanTask, long>, ITaskRepository
    {
        private readonly AppDbContext _contextk;

        public TaskRepository(AppDbContext context) : base(context)
        {
            _contextk = context;
        }

        public async Task<IEnumerable<KanbanTask>> GetByTeamIdAsync(long teamId)
        {
            return await _contextk.Tasks
                .Include(t => t.AssignedStudents)
                .Where(t => t.TeamId == teamId)
                .ToListAsync();
        }
    }
}
