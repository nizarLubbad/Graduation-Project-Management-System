using GPMS.Models;
using GPMS.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class LinkRepository : BaseRepository<Link>, ILinkRepository
    {
        private readonly AppDbContext _context;

        public LinkRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Link>> GetByTeamIdAsync(long teamId)
        {
            return await _context.Links
                                 .Include(l => l.Student)
                                 .Where(l => l.TeamId == teamId)
                                 .ToListAsync();
        }
    }
}
