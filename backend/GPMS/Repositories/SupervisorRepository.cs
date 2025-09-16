using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class SupervisorRepository : BaseRepository<Supervisor>, ISupervisorRepository
    {
        private  readonly AppDbContext _contextR;
        public SupervisorRepository(AppDbContext context) : base(context)
        {
            _contextR = context;
        }

        public async Task<Supervisor?> GetByEmailAsync(string email)
        {
            return await _context.Supervisors.FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}
