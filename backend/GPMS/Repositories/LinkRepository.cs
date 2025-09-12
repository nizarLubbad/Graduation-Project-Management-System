using GPMS.Models;
using GPMS.Interfaces;

namespace GPMS.Repositories
{
    public class LinkRepository : BaseRepository<Link>, ILinkRepository
    {
        public LinkRepository(AppDbContext context) : base(context) { }
    }
}