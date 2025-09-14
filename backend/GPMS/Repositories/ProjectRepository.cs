using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        private readonly AppDbContext _contextR;
        public ProjectRepository(AppDbContext context) : base(context)
        {
            _contextR = context;
        }

        public async Task<bool?> GetProjectStatusAsync(string projectTitle)
        {
            var project = await _contextR.Projects
                              .AsNoTracking()
                              .FirstOrDefaultAsync(p => p.ProjectTitle == projectTitle);

            return project?.projectStatus;
        }

        public async Task<string> GetProjectTitleAsync(int projectId)
        {
            var project = await _contextR.Projects
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            return project.ProjectTitle;
        }

        public async Task<bool?> UpdateProjectStatusAsync(string projectTitle)
        {
            var project = await _contextR.Projects
                                  .FirstOrDefaultAsync(p => p.ProjectTitle == projectTitle);

            if (project == null)
                return null; 

            project.projectStatus = true;

            
            await _contextR.SaveChangesAsync();

            return true;
        }
    }
}
