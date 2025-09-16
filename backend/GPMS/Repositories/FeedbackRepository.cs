using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class FeedbackRepository : BaseRepository<Feedback>, IFeedbackRepository
    {
        private readonly AppDbContext _context;

        public FeedbackRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Feedback>> GetByTeamIdAsync(long teamId)
        {
            return await _context.Feedbacks
                                 .Include(f => f.Supervisor)
                                 .Include(f => f.Replies)
                                 .Where(f => f.TeamId == teamId)
                                 .ToListAsync();
        }
    }
}
