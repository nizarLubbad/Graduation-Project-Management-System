using GPMS.Models;
using GPMS.Interfaces;

namespace GPMS.Repositories
{
    public class KanbanTaskRepository : BaseRepository<KanbanTask>, IKanbanTaskRepository
    {
        public KanbanTaskRepository(AppDbContext context) : base(context) { }
    }
}