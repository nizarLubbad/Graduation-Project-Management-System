using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class StudentRepository : BaseRepository<Student, long>, IStudentRepository
    {
        private readonly AppDbContext _contextR;
        public StudentRepository(AppDbContext context) : base(context)
        {
            _contextR = context;
        }

        public async Task<string?> GetByEmailAsync(string email)
        {
            var studentName = await _contextR.Users
               .Where(s => s.Email == email)
               .Select(s => s.Name)
               .FirstOrDefaultAsync();

            return studentName;
        }

        public async Task<string?> GetStudentNameAsync(long studentId)
        {
            var studentName = await _contextR.Users
                .Where(s => s.UserId == studentId)
                .Select(s => s.Name)
                .FirstOrDefaultAsync();

            return studentName;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _contextR.Users.AnyAsync(s => s.Email == email);
        }

        //public async Task SaveChangesAsync()
        //{
        //    await _contextR.SaveChangesAsync();
        //}
        public async Task<Student?> GetByUserIdAsync(long userId)
        {
            return await _contextR.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public IQueryable<Student> GetAll()
        {
            return _context.Students
                .Include(s => s.User)
                .Include(s => s.Team)
                .AsQueryable();
        }

    }
}
