using GPMS.DTOS.Reply;
using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IReplyService : IReplyRepository
    {
        Task<Reply> AddReplyAsync(CreateReplyDto dto);
        Task<IEnumerable<Reply>> GetByFeedbackIdAsync(int feedbackId);

    }
}
