using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface ILinkRepository : IBaseRepository<Link, long>
    {
        Task<IEnumerable<Link>> GetByTeamIdAsync(long teamId);
    }
}
