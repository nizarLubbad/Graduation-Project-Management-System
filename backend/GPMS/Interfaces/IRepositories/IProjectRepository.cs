using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task UpdateStatusToTrueAsync(int projectId);

        Task<bool?> GetStatusAsync(int projectId);
    }
}
