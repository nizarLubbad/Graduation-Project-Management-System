using GPMS.DTOS.Link;

namespace GPMS.Interfaces
{
    public interface ILinkService : IBaseService<LinkDto>
    {
        Task<IEnumerable<LinkDto>> GetByTeamIdAsync(long teamId);
    }
}
