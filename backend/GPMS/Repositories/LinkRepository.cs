using GPMS.Models;
using GPMS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class LinkRepository : BaseRepository<Link, long>, ILinkRepository
    {
        private readonly AppDbContext _contextl;

        public LinkRepository(AppDbContext context) : base(context)
        {
            _contextl = context;
        }

        public async Task<IEnumerable<Link>> GetByTeamIdAsync(long teamId)
        {
            return await _contextl.Links
                                 .Include(l => l.Student)
                                 .Where(l => l.TeamId == teamId)
                                 .ToListAsync();
        }
    }
}
