using GPMS.DTOS.Reply;
using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IReplyService 
    {
        Task<ReplyResponseDto> CreateAsync(CreateReplyDto dto);
        Task<ReplyResponseDto?> UpdateAsync(long replyId, UpdateReplyDto dto);
        Task<bool> DeleteAsync(long replyId);
        Task<ReplyResponseDto?> GetByIdAsync(long replyId);
        Task<IEnumerable<ReplyResponseDto>> GetByFeedbackIdAsync(long feedbackId);
    }
}
