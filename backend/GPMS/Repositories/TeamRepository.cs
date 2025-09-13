using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GPMS.Repositories
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        private readonly AppDbContext _contextR;
        public TeamRepository(AppDbContext context) : base(context)
        {
            _contextR = context;

        }

        public async Task<string?> GetTeamNmaeAsync(long TeamId)
        {

            var team = await _contextR.Teams
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(t => t.TeamId == TeamId);

            return team?.TeamName;
        }

        
    }
}
