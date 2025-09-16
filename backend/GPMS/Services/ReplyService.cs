using GPMS.DTOS.Reply;
using GPMS.Interfaces;
using GPMS.Models;

namespace GPMS.Services
{
    public class ReplyService : IReplyService
    {
        private readonly IReplyRepository _replyRepository;

        public ReplyService(IReplyRepository replyRepository)
        {
            _replyRepository = replyRepository;
        }

        public async Task<Reply> AddReplyAsync(CreateReplyDto dto)
        {
            var reply = new Reply
            {
                Message = dto.Message,
                FeedbackId = dto.FeedbackId,
                StudentId = dto.StudentId,
                SupervisorId = dto.SupervisorId,
                Date = DateTime.Now
            };

            return await _replyRepository.AddAsync(reply);
        }

        public async Task<IEnumerable<Reply>> GetByFeedbackIdAsync(int feedbackId)
        {
            return await _replyRepository.GetByFeedbackIdAsync(feedbackId);
        }
    }
}
