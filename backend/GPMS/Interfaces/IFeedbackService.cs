using GPMS.DTOS.Feedback;
using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IFeedbackService : IFeedbackRepository
    {
        Task<Feedback> AddFeedbackAsync(CreateFeedbackDto dto);
        Task<IEnumerable<Feedback>> GetByTeamIdAsync(long teamId);
        Task<Feedback?> GetByIdAsync(int id);

    }
}
