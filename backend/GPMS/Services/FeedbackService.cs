using AutoMapper;
using GPMS.DTOS.Feedback;
using GPMS.Interfaces;
using GPMS.Models;

namespace GPMS.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMapper _mapper;
        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;

        }


        public async Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto dto)
        {
            var feedback = new Feedback
            {
                Content = dto.Content,
                TeamId = dto.TeamId,
                SupervisorId = dto.SupervisorId,
                Date = DateTime.Now
            };

            var savedFeedback = await _feedbackRepository.AddAsync(feedback);
            return _mapper.Map<FeedbackDto>(savedFeedback);

        }

        public async Task<bool> DeleteFeedbackAsync(long feedbackId)
        {
            return await _feedbackRepository.DeleteAsync(feedbackId);
        }

        public async Task<FeedbackDto?> GetByIdAsync(long feedbackId)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
            return _mapper.Map<FeedbackDto?>(feedback);
        }

        // update feedback //feedbackid
        //delete feedback //feedbackid



        public async Task<IEnumerable<FeedbackDto>> GetByTeamIdAsync(long teamId)
        {
            //var allFeedbacks = await _feedbackRepository.GetAllAsync();

            //var teamFeedbacks = allFeedbacks.Where(f => f.TeamId == teamId);

            //return _mapper.Map<IEnumerable<FeedbackDto>>(teamFeedbacks);
            var feedbacks = await _feedbackRepository.GetByTeamIdAsync(teamId);
            return _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
        }

        //public async Task<FeedbackDto?> UpdateFeedbackAsync(int feedbackId, UpdateFeedbackDto dto)
        //{
        //    var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
        //    if (feedback == null) return null;

        //    feedback.Content = dto.Content;
        //    feedback.Date = DateTime.UtcNow;

        //    var updated = await _feedbackRepository.UpdateAsync(feedback);
        //    return _mapper.Map<FeedbackDto>(updated);
        //}

        public async Task<FeedbackDto?> UpdateFeedbackAsync(long feedbackId, UpdateFeedbackDto dto)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
            if (feedback == null) return null;

            feedback.Content = dto.Content;
            feedback.Date = DateTime.UtcNow;

            var updated = await _feedbackRepository.UpdateAsync(feedback);
            return _mapper.Map<FeedbackDto>(updated);
        }
    }
}
