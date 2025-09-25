using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Repositories
{
    public class UserRepository : BaseRepository<User, long>, IUserRepository
    {
        
        private readonly AppDbContext _contextU;

        public UserRepository(AppDbContext context) : base(context)
        {
            _contextU = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _contextU.Users
                .Include(u => u.Student)
                .Include(u => u.Supervisor)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _contextU.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Student>> GetStudentsByIdsAsync(List<long> studentIds)
        {
            return await _context.Students
                .Include(s => s.User) 
                .Where(s => studentIds.Contains(s.UserId))
                .Where(s => s.User != null && s.User.Role == "Student")
                .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _contextU.Users
                .Include(u => u.Student)
                .Include(u => u.Supervisor).OrderBy(u => u.Name)
                .ToListAsync();
            //.Where(u => u.Role == "Student") 

        }

        public async  Task<User?> GetByIdWithDetailsAsync(long userId)
        {
            return await _contextU.Users
               .Include(u => u.Student)
               .Include(u => u.Supervisor)
               .FirstOrDefaultAsync(u => u.UserId == userId);
        }
        public override async Task<bool> DeleteAsync(long userId)
        {
            try
            {
                var user = await _contextU.Users
                    .Include(u => u.Student)
                        .ThenInclude(s => s.Links)
                    .Include(u => u.Student)
                        .ThenInclude(s => s.Replies)
                    .Include(u => u.Supervisor)
                        .ThenInclude(s => s.Teams)
                        .ThenInclude(t => t.Project)
                    .Include(u => u.Supervisor)
                        .ThenInclude(s => s.Feedbacks)
                    .Include(u => u.Supervisor)
                        .ThenInclude(s => s.Replys)
                    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null) return false;

                if (user.Role == "Student")
                {
                    await DeleteStudentDataAsync(user);
                }
                else if (user.Role == "Supervisor")
                {
                    await DeleteSupervisorDataAsync(user);
                }

                _contextU.Users.Remove(user);
                await _contextU.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _contextU.ChangeTracker.Clear();
                throw new Exception($"An error occurred while deleting user {userId}: {ex.Message}", ex);
            }
        }

        private async Task DeleteStudentDataAsync(User user)
        {
            if (user.Student == null) return;

            var studentLinks = await _contextU.Links
                .Where(l => l.StudentId == user.UserId)
                .ToListAsync();
            _contextU.Links.RemoveRange(studentLinks);

            var studentReplies = await _contextU.Replys
                .Where(r => r.StudentId == user.UserId)
                .ToListAsync();
            _contextU.Replys.RemoveRange(studentReplies);

            if (user.Student.TeamId != null)
            {
                user.Student.TeamId = null;
                user.Student.Status = false;
            }

            _contextU.Students.Remove(user.Student);
        }

        private async Task DeleteSupervisorDataAsync(User user)
        {
            if (user.Supervisor == null) return;

            var supervisorTeams = await _contextU.Teams
                .Include(t => t.Project)
                .Include(t => t.Students)
                .Where(t => t.SupervisorId == user.UserId)
                .ToListAsync();

            foreach (var team in supervisorTeams)
            {
                foreach (var student in team.Students)
                {
                    student.TeamId = null;
                    student.Status = false;
                }

                if (team.Project != null)
                {
                    _contextU.Projects.Remove(team.Project);
                }

                _contextU.Teams.Remove(team);
            }

            var supervisorFeedbacks = await _contextU.Feedbacks
                .Where(f => f.SupervisorId == user.UserId)
                .ToListAsync();
            _contextU.Feedbacks.RemoveRange(supervisorFeedbacks);

            var supervisorReplies = await _contextU.Replys
                .Where(r => r.SupervisorId == user.UserId)
                .ToListAsync();
            _contextU.Replys.RemoveRange(supervisorReplies);

            _contextU.Supervisors.Remove(user.Supervisor);
        }

        public async Task<int> DeleteAllUsersAsync()
        {
            try
            {
                var links = await _contextU.Links.ToListAsync();
                _contextU.Links.RemoveRange(links);

                var replies = await _contextU.Replys.ToListAsync();
                _contextU.Replys.RemoveRange(replies);

                var feedbacks = await _contextU.Feedbacks.ToListAsync();
                _contextU.Feedbacks.RemoveRange(feedbacks);

                var projects = await _contextU.Projects.ToListAsync();
                _contextU.Projects.RemoveRange(projects);

                var teams = await _contextU.Teams.ToListAsync();
                _contextU.Teams.RemoveRange(teams);

                var students = await _contextU.Students.ToListAsync();
                _contextU.Students.RemoveRange(students);

                var supervisors = await _contextU.Supervisors.ToListAsync();
                _contextU.Supervisors.RemoveRange(supervisors);

                var users = await _contextU.Users.ToListAsync();
                _contextU.Users.RemoveRange(users);

                return await _contextU.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _contextU.ChangeTracker.Clear();
                throw new Exception("An error occurred while deleting all users", ex);
            }
        }
       
    
    }
}
