using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class SupervisorRepository : BaseRepository<Supervisor>, ISupervisorRepository
    {
        private readonly AppDbContext _contextR;
        public SupervisorRepository(AppDbContext context) : base(context)
        {
            _contextR = context;
        }

        public async Task<string?> GetByEmailAsync(string email)
        {
            var supervisorName = await _contextR.Supervisors
                .Where(s => s.Email == email)
                .Select(s => s.Name)
                .FirstOrDefaultAsync();

            return supervisorName;
        }

        public async Task<string?> GetSupervisorNameAsync(long supervisorId)
        {
            var supervisorName = await _contextR.Supervisors
                .Where(s => s.SupervisorId == supervisorId)
                .Select(s => s.Name)
                .FirstOrDefaultAsync();

            return supervisorName;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _contextR.Supervisors.AnyAsync(s => s.Email == email);
        }

        public async Task SaveChangesAsync()
        {
            await _contextR.SaveChangesAsync();
        }

       

        //public async Task<Supervisor?> GetByEmailAsync(string email)
        //{
        //    return await _contextR.Supervisors
        //        .FirstOrDefaultAsync(s => s.Email == email);
        //}

    }
}
