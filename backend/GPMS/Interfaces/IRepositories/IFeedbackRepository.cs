using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IFeedbackRepository : IBaseRepository<Feedback, long>
    {
        Task<IEnumerable<Feedback>> GetByTeamIdAsync(long teamId);
    }
}
