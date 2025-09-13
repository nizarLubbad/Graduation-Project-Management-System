using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool?> GetProjectStatusAsync(string projectTitle)
        {
            var project = await _context.Projects
                              .AsNoTracking()
                              .FirstOrDefaultAsync(p => p.ProjectTitle == projectTitle);

            return project?.projectStatus;
        }

        public async Task<string> GetProjectTitleAsync(int projectId)
        {
            var project = await _context.Projects
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            return project.ProjectTitle;
        }


    }
}
