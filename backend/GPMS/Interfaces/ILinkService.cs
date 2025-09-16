using GPMS.DTOS.Link;
using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface ILinkService
    {
        Task<Link> AddLinkAsync(CreateLinkDto dto);
        Task<IEnumerable<Link>> GetLinksByTeamIdAsync(long teamId);
        Task<Link?> UpdateLinkAsync(int id, LinkDto dto);
        Task<bool> DeleteLinkAsync(int id);
    }
}
