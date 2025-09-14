using GPMS.DTOS.Project;
using GPMS.DTOS.Student;

namespace GPMS.Interfaces
{
    public interface ITeamService : ITeamRepository
    {
        Task UpdateStudentsStatusForTeamAsync(long teamId);
        Task<TeamDto> CreateTeamAsync(long creatorStudentId, IEnumerable<long> memberStudentIds);
        Task<IEnumerable<StudentDto>> GetStudentsByTeamIdAsync(long teamId);


    }
}
