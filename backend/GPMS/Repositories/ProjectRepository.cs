using GPMS.Models;
using GPMS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class ProjectRepository :BaseRepository<Project,int> ,IProjectRepository
    {
        private readonly AppDbContext _contextp;

        public ProjectRepository(AppDbContext context) : base(context)
        {
            _contextp = context;
        }

     


        public async Task UpdateStatusToTrueAsync(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null) return;

            project.IsCompleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<bool?> GetStatusAsync(int projectId)
        {
            var project = await _context.Projects
                 .AsNoTracking()
                 .FirstOrDefaultAsync(p => p.Id == projectId);

            return project?.IsCompleted;
        }
        ///////////////
        ///

        public override async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _contextp.Projects
                 .Include(p => p.Team!)
                 .ThenInclude(t => t!.Supervisor)
                 .Where(p => p.Team != null) 
                 .ToListAsync();
        }

        public override async Task<Project?> GetByIdAsync(int id)
        {
            return await _contextp.Projects
                 .Include(p => p.Team!)
                 .ThenInclude(t => t!.Supervisor)
                 .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Team?> GetTeamWithSupervisorAsync(long teamId)
        {
            return await _contextp.Teams
                .Include(t => t.Supervisor)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(t => t.TeamId == teamId);
        }


    }


}

