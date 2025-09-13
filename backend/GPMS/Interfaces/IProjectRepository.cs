using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IProjectRepository : IBaseRepository<Project>
    {
        Task<bool?> GetProjectStatusAsync(string projectTitle);
        Task<string> GetProjectTitleAsync(int projectId);
    }
}
