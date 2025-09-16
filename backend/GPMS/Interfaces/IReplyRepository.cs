using GPMS.DTOS.Reply;
using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IReplyRepository : IBaseRepository<Reply>
    {
        Task<IEnumerable<Reply>> GetByFeedbackIdAsync(int feedbackId);

        //Task<Reply> AddAsync(Reply reply);
    }
}
