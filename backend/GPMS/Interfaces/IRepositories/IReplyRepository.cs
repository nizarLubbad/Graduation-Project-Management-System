using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IReplyRepository : IBaseRepository<Reply>
    {
        Task<IEnumerable<Reply>> GetByFeedbackIdAsync(long feedbackId);
    }
}
