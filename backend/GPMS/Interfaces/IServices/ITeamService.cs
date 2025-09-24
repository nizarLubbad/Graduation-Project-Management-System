using GPMS.DTOS.Student;
using GPMS.DTOS.Team;


namespace GPMS.Interfaces
{
    public interface ITeamService 
    {
        //Task<IEnumerable<TeamDto>> GetAllAsync();
        //Task<TeamDto?> GetByIdAsync(long teamId);
        //Task<TeamDto> CreateAsync(CreateTeamDto dto);
        //Task<TeamDto?> UpdateAsync(long teamId, UpdateTeamDto dto);
        ////Task<bool> DeleteAsync(long teamId);
        //Task UpdateStudentsStatusForTeamAsync(long teamId);
        //Task<TeamDto> CreateTeamAsync(long creatorStudentId, IEnumerable<long> memberStudentIds);
        //Task<IEnumerable<StudentDto>> GetStudentsByTeamIdAsync(long teamId);
        Task<IEnumerable<TeamDto>> GetAllAsync();
        Task<TeamDto> CreateTeamAsync(string teamName, IEnumerable<long> memberStudentIds);

        Task<TeamDto?> GetByIdAsync(long teamId);
        //Task<TeamDto> CreateAsync(long creatorStudentId, IEnumerable<long> memberStudentIds, string teamName);
        Task<bool> DeleteAsync(long teamId);
        Task UpdateStudentsStatusAsync(long teamId);
        Task<List<StudentDto>> GetTeamMembersByStudentIdAsync(long studentId);

    }
}
