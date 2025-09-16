using GPMS.Models;
using GPMS.DTOS.Link;

namespace GPMS.Interfaces
{
    public interface ILinkService
    {
        Task<Link> AddLinkAsync(CreateLinkDto dto);
        Task<IEnumerable<Link>> GetByTeamIdAsync(long teamId);
        Task<Link?> GetByIdAsync(long id);
        Task<bool> DeleteAsync(long id);
    }
}
