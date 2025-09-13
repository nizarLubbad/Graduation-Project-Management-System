using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GPMS.Repositories
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        private readonly AppDbContext _context;
        public TeamRepository(AppDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<string?> GetTeamNmaeAsync(long TeamId)
        {

            var team = await _context.Teams
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(t => t.TeamId == TeamId);

            return team?.TeamName;
        }

        
    }
}
