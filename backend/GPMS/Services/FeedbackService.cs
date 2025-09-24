using AutoMapper;
using GPMS.DTOS.Feedback;
using GPMS.Interfaces;
using GPMS.Models;
using GPMS.Repositories;

namespace GPMS.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper, ITeamRepository teamRepository, ISupervisorRepository supervisorRepository)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _teamRepository = teamRepository;
            _supervisorRepository = supervisorRepository;
        }


        public async Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto dto)
        {
            var team = await _teamRepository.GetTeamWithDetailsAsync(dto.TeamId);
            if (team == null)
                throw new KeyNotFoundException("Team not found");

            var isSupervisorOfTeam = await _supervisorRepository.IsSupervisorOfTeamAsync(dto.SupervisorId, dto.TeamId);
            if (!isSupervisorOfTeam)
                throw new UnauthorizedAccessException("Supervisor cannot add feedback to this team");

            var feedback = new Feedback
            {
                Content = dto.Content,
                TeamId = dto.TeamId,
                SupervisorId = dto.SupervisorId,
                Date = DateTime.Now
            };

            await _feedbackRepository.AddAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();

            return _mapper.Map<FeedbackDto>(feedback);
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
