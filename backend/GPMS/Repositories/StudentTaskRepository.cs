using GPMS.Models;
using GPMS.Interfaces;

namespace GPMS.Repositories
{
    public class StudentTaskRepository : BaseRepository<StudentTask>, IStudentTaskRepository
    {
        public StudentTaskRepository(AppDbContext context) : base(context) { }
    }
}