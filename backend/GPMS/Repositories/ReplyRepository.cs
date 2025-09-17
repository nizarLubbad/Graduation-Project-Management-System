using GPMS.Models;
using GPMS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class ReplyRepository : BaseRepository<Reply>, IReplyRepository
    {
        private readonly AppDbContext _context;

        public ReplyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reply>> GetByFeedbackIdAsync(long feedbackId)
        {
            return await _context.Replies
                .Include(r => r.Student)
                .Include(r => r.Supervisor)
                .Where(r => r.FeedbackId == feedbackId)
                .ToListAsync();
        }
    }
}
