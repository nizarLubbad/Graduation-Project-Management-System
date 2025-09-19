using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace GPMS.Repositories
{
    public class TeamRepository : BaseRepository<Team>, ITeamRepository
    {
        private readonly AppDbContext _contextR;
        public TeamRepository(AppDbContext context) : base(context)
        {
            _contextR = context;

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
                .Where(s => allStudentIds.Contains(s.StudentId))
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
    }
}
