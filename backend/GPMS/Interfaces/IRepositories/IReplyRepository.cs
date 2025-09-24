using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IReplyRepository : IBaseRepository<Reply, long>
    {
        Task<IEnumerable<Reply>> GetByFeedbackIdAsync(long feedbackId);
    }
}
