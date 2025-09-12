using GPMS.Interfaces;
using GPMS.Models;

namespace GPMS.Repositories
{
    public class FeedbackRepository : BaseRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) : base(context) { }
    }
}