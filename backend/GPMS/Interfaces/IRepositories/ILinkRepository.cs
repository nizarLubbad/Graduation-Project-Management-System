using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface ILinkRepository : IBaseRepository<Link>
    {
        Task<IEnumerable<Link>> GetByTeamIdAsync(long teamId);
    }
}
