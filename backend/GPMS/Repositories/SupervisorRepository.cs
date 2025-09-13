using GPMS.Interfaces;
using GPMS.Models;

namespace GPMS.Repositories
{
    public class SupervisorRepository : BaseRepository<Supervisor>, ISupervisorRepository
    {
        private  readonly AppDbContext _context;
        public SupervisorRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
