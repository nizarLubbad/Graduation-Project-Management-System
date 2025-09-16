using GPMS.DTOS.Feedback;
using GPMS.Interfaces;
using GPMS.Models;

namespace GPMS.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<Feedback> AddFeedbackAsync(CreateFeedbackDto dto)
        {
            var feedback = new Feedback
            {
                Content = dto.Content,
                TeamId = dto.TeamId,
                SupervisorId = dto.SupervisorId,
                Date = DateTime.Now
            };

            return await _feedbackRepository.AddAsync(feedback);
        }

        public async Task<IEnumerable<Feedback>> GetByTeamIdAsync(long teamId)
        {
            return await _feedbackRepository.GetByTeamIdAsync(teamId);
        }

        public async Task<Feedback?> GetByIdAsync(int id)
        {
            return await _feedbackRepository.GetByIdAsync(id);
        }
    }
}
