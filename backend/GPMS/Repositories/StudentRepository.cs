using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        private readonly AppDbContext _contextR;
        public StudentRepository(AppDbContext context) : base(context) { 
               _contextR = context;
        }

        public async  Task<string?> GetStudentNameAsync(long studentId)
        {

            var studentName = await _contextR.Students
                .Where(s => s.StudentId == studentId)
                .Select(s => s.Name)
                .FirstOrDefaultAsync(); 

            return studentName;
        }
    }
}