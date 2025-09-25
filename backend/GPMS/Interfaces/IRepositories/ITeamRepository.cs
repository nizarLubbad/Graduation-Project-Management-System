using GPMS.Models;
using System.Diagnostics.Eventing.Reader;

namespace GPMS.Interfaces
{
    public interface ITeamRepository : IBaseRepository<Team, long>
    {
        Task<string?> GetTeamNameAsync(long teamId);

        // Get team with students and supervisor included
        Task<Team?> GetTeamWithDetailsAsync(long teamId);
        Task<IEnumerable<Student>> GetTeamStudentsAsync(long teamId);
        Task<IEnumerable<Student>> GetTeamMembersByStudentIdAsync(long studentId);

        // Create team with students status update
        //Task<Team> CreateTeamWithStudentsAsync(long creatorStudentId, IEnumerable<long> memberStudentIds, string teamName);
        //Task<string?> GetTeamNmaeAsync(long TeamId);
    }
}