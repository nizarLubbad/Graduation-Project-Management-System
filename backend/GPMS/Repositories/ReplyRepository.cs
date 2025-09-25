using GPMS.Models;
using GPMS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class ReplyRepository : BaseRepository<Reply, long>, IReplyRepository
    {
        private readonly AppDbContext _contextr;

        public ReplyRepository(AppDbContext context) : base(context)
        {
            _contextr = context;
        }

        public async Task<IEnumerable<Reply>> GetByFeedbackIdAsync(long feedbackId)
        {
            return await _contextr.Replys
                .Include(r => r.Student)
                .Include(r => r.Supervisor)
                .Where(r => r.FeedbackId == feedbackId)
                .ToListAsync();
        }
        //Task<IEnumerable<Reply>> GetByFeedbackIdAsync(long feedbackId);

    }
}
