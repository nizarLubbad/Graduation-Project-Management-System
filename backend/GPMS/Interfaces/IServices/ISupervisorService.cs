using GPMS.DTOS.Project;
using GPMS.DTOS.Supervisor;


namespace GPMS.Interfaces
{
    public interface ISupervisorService 
    {
        ////Task<IEnumerable<TDto>> GetAllAsync();
        ////Task<TDto?> GetByIdAsync(int id);
        ////Task<SupervisorDto> CreateAsync(SupervisorDto dto);
        //Task<SupervisorDto?> UpdateAsync(int id, SupervisorDto dto);
        ////Task<bool> DeleteAsync(int id);
        ////Task<SupervisorDto?> GetByEmailAsync(string email);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        Task<int?> GetRemainingTeamsAsync(long userId);
        //Task<IEnumerable<SupervisorDto>> GetAllAsync();
        Task<bool> SetMaxTeamsAsync(long userId, int maxTeams);
        Task<bool> IncrementTeamCountAsync(long userId);
        Task<bool> DecrementTeamCountAsync(long userId);
        Task<List<SupervisorDto>> GetAllSupervisorsAsync();
        //Task<SupervisorProfileDto?> GetSupervisorProfileAsync(long userId);
        Task<(bool Success, string ErrorMessage)> BookTeamAsync(long supervisorId, long teamId);
        Task<int?> GetMaxTeamsAsync(long userId);


    }
}
