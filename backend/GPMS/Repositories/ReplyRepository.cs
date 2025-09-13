using GPMS.Models;

namespace GPMS.Repositories
{
    public class ReplyRepository : BaseRepository<Reply>
    {
        private readonly AppDbContext _contextR;
        public ReplyRepository(AppDbContext context) : base(context)
        {
            _contextR = context;
        }
    }
}
