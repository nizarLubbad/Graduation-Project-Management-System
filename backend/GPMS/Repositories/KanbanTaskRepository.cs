using GPMS.Models;
using GPMS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class KanbanTaskRepository : BaseRepository<KanbanTask>, IKanbanTaskRepository
    {
        private readonly AppDbContext _context;
        public KanbanTaskRepository(AppDbContext context) : base(context) {
            _context = context;
        }

        public Task<string?> GetTaskStatusAsync(int taskId)
        {
            var task = _context.Tasks
                               .AsNoTracking()
                               .FirstOrDefault(t => t.TaskId == taskId);
            return Task.FromResult(task?.status);
        }
    }
}