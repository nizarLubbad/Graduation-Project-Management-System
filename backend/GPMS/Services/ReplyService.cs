using AutoMapper;
using GPMS.DTOS.Reply;
using GPMS.Interfaces;
using GPMS.Models;

namespace GPMS.Services
{
    //public class ReplyService : IReplyService
    //{
    //    private readonly IReplyRepository _replyRepository;
    //    private readonly IMapper _mapper;

    //    public ReplyService(IReplyRepository replyRepository,IMapper mapper)
    //    {
    //        _replyRepository = replyRepository;
    //        _mapper = mapper;


    //    }

    //    public async Task<Reply> AddReplyAsync(CreateReplyDto dto)
    //    {
    //        var reply = new Reply
    //        {
    //            Content = dto.Content,
    //            FeedbackId = dto.FeedbackId,
    //            StudentId = dto.StudentId,
    //            SupervisorId = dto.SupervisorId,
    //            Date = DateTime.Now
    //        };

    //        return await _replyRepository.AddAsync(reply);
    //    }

    //    public async Task<IEnumerable<Reply>> GetByFeedbackIdAsync(long feedbackId)
    //    {
    //        return await _replyRepository.GetByFeedbackIdAsync(feedbackId);
    //    }
    //}
    public class ReplyService : IReplyService
    {
        private readonly IReplyRepository _replyRepository;
        private readonly IMapper _mapper;

        public ReplyService(IReplyRepository replyRepository, IMapper mapper)
        {
            _replyRepository = replyRepository;
            _mapper = mapper;
        }

        public async Task<ReplyResponseDto> CreateAsync(CreateReplyDto dto)
        {
            var reply = new Reply
            {
                Content = dto.Content,
                FeedbackId = dto.FeedbackId,
                StudentId = dto.StudentId,
                SupervisorId = dto.SupervisorId,
                Date = DateTime.Now
            };

            var saved = await _replyRepository.AddAsync(reply);
            return _mapper.Map<ReplyResponseDto>(saved);
        }

        public async Task<ReplyResponseDto?> UpdateAsync(long replyId, UpdateReplyDto dto)
        {
            var reply = await _replyRepository.GetByIdAsync(replyId);
            if (reply == null) return null;

            reply.Content = dto.Content;
            reply.Date = DateTime.Now; 

            var updated = await _replyRepository.UpdateAsync(reply);
            return _mapper.Map<ReplyResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(long replyId)
        {
            return await _replyRepository.DeleteAsync(replyId);
        }

        public async Task<ReplyResponseDto?> GetByIdAsync(long replyId)
        {
            var reply = await _replyRepository.GetByIdAsync(replyId);
            if (reply == null) return null;

            return _mapper.Map<ReplyResponseDto>(reply);
        }

        public async Task<IEnumerable<ReplyResponseDto>> GetByFeedbackIdAsync(long feedbackId)
        {
            var replies = await _replyRepository.GetByFeedbackIdAsync(feedbackId);
            return _mapper.Map<IEnumerable<ReplyResponseDto>>(replies);
        }
    }
}
