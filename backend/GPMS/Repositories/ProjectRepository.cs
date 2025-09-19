using GPMS.Models;
using GPMS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class ProjectRepository :BaseRepository<Project> ,IProjectRepository
    {
        private readonly AppDbContext _contextp;

        public ProjectRepository(AppDbContext context) : base(context)
        {
            _contextp = context;
        }

        //public async Task<IEnumerable<Project>> GetAllAsync()
        //{
        //    return await _context.Projects
        //                         .Include(p => p.Supervisor)
        //                         .Include(p => p.Students)
        //                         .ToListAsync();
        //}

        //public async Task<Project?> GetByIdAsync(int id)
        //{
        //    return await _context.Projects
        //                         .Include(p => p.Supervisor)
        //                         .Include(p => p.Students)
        //                         .FirstOrDefaultAsync(p => p.Id == id);
        //}

        //public async Task<Project> AddAsync(Project project)
        //{
        //    _context.Projects.Add(project);
        //    await _context.SaveChangesAsync();
        //    return project;
        //}

        //public async Task<Project> UpdateAsync(Project project)
        //{
        //    _context.Projects.Update(project);
        //    await _context.SaveChangesAsync();
        //    return project;
        //}

        //public async Task<bool> DeleteAsync(string projectTitle)
        //{
        //    var project = await _context.Projects.FindAsync(projectTitle);
        //    if (project == null) return false;

        //    _context.Projects.Remove(project);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public Task<Project?> GetByIdAsync(object id)
        //{
        //    throw new NotImplementedException();
        //}



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
    }
}
