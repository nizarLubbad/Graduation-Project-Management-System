using GPMS.DTOS.Project;
using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class SupervisorRepository : BaseRepository<Supervisor, long>, ISupervisorRepository
    {
        private readonly AppDbContext _contextR;
        public SupervisorRepository(AppDbContext context) : base(context)
        {
            _contextR = context;
        }

        public async Task<string?> GetByEmailAsync(string email)
        {
            var supervisorName = await _contextR.Users
                .Where(s => s.Email == email)
                .Select(s => s.Name)
                .FirstOrDefaultAsync();

            return supervisorName;
        }

        public async Task<IEnumerable<SupervisorDto>> GetAllSupervisorsAsync()
        {
            return await _contextR.Supervisors
                .Include(s => s.User) 
                .Select(s => new SupervisorDto
                {
                    SupervisorId = s.UserId,
                    Name = s.User.Name,
                    Email = s.User.Email,
                    //Department = s.Department,
                    TeamCount = s.TeamCount,
                    MaxTeams = s.MaxTeams
                })
                .ToListAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _contextR.Users.AnyAsync(s => s.Email == email);
        }

        //public async Task SaveChangesAsync()
        //{
        //    await _contextR.SaveChangesAsync();
        //}


        public async Task<Supervisor?> GetByUserIdAsync(long userId)
        {
            return await _context.Supervisors
               .Include(s => s.User)
               .Include(s => s.Teams)
               .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public IQueryable<Supervisor> GetAll()
        {
            return _context.Supervisors
                .Include(s => s.User)
                .Include(s => s.Teams)
                .AsQueryable();
        }
        public async Task<bool> IsSupervisorOfTeamAsync(long supervisorId, long teamId)
        {
            return await _contextR.Teams
                .AnyAsync(t => t.TeamId == teamId && t.SupervisorId == supervisorId);
        }

        //Task<Supervisor?> ISupervisorRepository.GetByEmailAsync(string email)
        //{
        //    throw new NotImplementedException();
        //}
        //public async Task<Supervisor?> GetByEmailAsync(string email)
        //{
        //    return await _contextR.Supervisors
        //        .FirstOrDefaultAsync(s => s.Email == email);
        //}

    }
}
