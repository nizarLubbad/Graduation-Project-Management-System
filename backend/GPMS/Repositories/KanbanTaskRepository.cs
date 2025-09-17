using GPMS.Models;
using GPMS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class TaskRepository : BaseRepository<KanbanTask>, ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KanbanTask>> GetByTeamIdAsync(long teamId)
        {
            return await _context.Tasks
                .Include(t => t.AssignedStudents)
                .Where(t => t.TeamId == teamId)
                .ToListAsync();
        }
    }
}
