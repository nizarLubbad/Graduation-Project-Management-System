using GPMS.Models;
using GPMS.Interfaces;

namespace GPMS.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context) { }
    }
}