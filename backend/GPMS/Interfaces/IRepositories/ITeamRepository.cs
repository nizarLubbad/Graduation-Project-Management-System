using GPMS.Models;
using System.Diagnostics.Eventing.Reader;

namespace GPMS.Interfaces
{
    public interface ITeamRepository : IBaseRepository<Team>
    {
        Task<string?> GetTeamNameAsync(long teamId);

        // Get team with students and supervisor included
        Task<Team?> GetTeamWithDetailsAsync(long teamId);

        // Create team with students status update
        Task<Team> CreateTeamWithStudentsAsync(long creatorStudentId, IEnumerable<long> memberStudentIds, string teamName);
        //Task<string?> GetTeamNmaeAsync(long TeamId);
    }
}