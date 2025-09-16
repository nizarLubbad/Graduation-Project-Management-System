using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IFeedbackRepository : IBaseRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetByTeamIdAsync(long teamId);
    }
}