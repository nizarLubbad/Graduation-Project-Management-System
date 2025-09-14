using GPMS.Models;
using System.Diagnostics.Eventing.Reader;

namespace GPMS.Interfaces
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        Task<string?> GetTeamNmaeAsync(long TeamId);
    }
}