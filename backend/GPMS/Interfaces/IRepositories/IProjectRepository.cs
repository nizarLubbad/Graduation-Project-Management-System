using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IProjectRepository : IBaseRepository<Project, int>
    {
        Task UpdateStatusToTrueAsync(int projectId);

        Task<bool?> GetStatusAsync(int projectId);
        Task<Team?> GetTeamWithSupervisorAsync(long teamId);

    }
}
