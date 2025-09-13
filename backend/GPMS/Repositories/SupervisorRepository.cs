using GPMS.Interfaces;
using GPMS.Models;

namespace GPMS.Repositories
{
    public class SupervisorRepository : BaseRepository<Supervisor>, ISupervisorRepository
    {
        private  readonly AppDbContext _contextR;
        public SupervisorRepository(AppDbContext context) : base(context)
        {
            _contextR = context;
        }


    }
}
