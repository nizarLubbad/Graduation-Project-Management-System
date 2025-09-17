using GPMS.DTOS.Reply;
using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IReplyService
    {
        Task<Reply> AddReplyAsync(CreateReplyDto dto);
        Task<IEnumerable<Reply>> GetByFeedbackIdAsync(long feedbackId);
    }
}
