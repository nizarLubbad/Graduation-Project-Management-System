using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GPMS.Repositories
{
    public class TeamRepository : BaseRepository<Team, long>, ITeamRepository
    {
        private readonly AppDbContext _contextR;
        public TeamRepository(AppDbContext context) : base(context)
        {
            _contextR = context;

        }
        public override async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _context.Teams
                .Include(t => t.Students)
                .Include(t => t.Supervisor)
                .Include(t => t.Project)
                .ToListAsync();
        }

        public async Task<Team?> GetTeamWithDetailsAsync(long teamId)
        {
            return await _context.Teams
                .AsNoTracking()  
                .Include(t => t.Students)
                .Include(t => t.Supervisor)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.TeamId == teamId);
        }
        public async Task<Team> CreateTeamWithStudentsAsync(long creatorStudentId, IEnumerable<long> memberStudentIds, string teamName)
        {
            var team = new Team
            {
                TeamId = creatorStudentId,
                TeamName = teamName,
                CreatedDate = DateTime.Now,
            };

            await _context.Teams.AddAsync(team);

            var allStudentIds = new List<long>(memberStudentIds) { creatorStudentId };

            var students = await _context.Students
                .Where(s => allStudentIds.Contains(s.UserId))
                .ToListAsync();

            foreach (var student in students)
            {
                student.Status = true;
                student.TeamId = team.TeamId;
            }

            _context.Students.UpdateRange(students);

            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<string?> GetTeamNameAsync(long teamId)
        {
            var team = await _contextR.Teams
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(t => t.TeamId == teamId);

            return team?.TeamName;
        }
        public async Task<IEnumerable<Student>> GetTeamStudentsAsync(long teamId)
        {
            return await _contextR.Students
                .Where(s => s.TeamId == teamId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Student>> GetTeamMembersByStudentIdAsync(long studentId)
        {
            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == studentId);

            if (student == null || student.TeamId == null)
                return new List<Student>(); 

            long teamId = student.TeamId.Value;

            var teamMembers = await _context.Students
                .AsNoTracking()
                .Include(s => s.User)
                .Where(s => s.TeamId == teamId)
                .ToListAsync();

            return teamMembers;
        }
    }
}

