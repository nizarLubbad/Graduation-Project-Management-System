using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface ISupervisorRepository : IBaseRepository<Supervisor, long>
    {
        //Task<Supervisor?> GetByEmailAsync(string email);
        //Task SaveChangesAsync();
        Task<bool> ExistsByEmailAsync(string email);
        Task<Supervisor?> GetByUserIdAsync(long userId);
        IQueryable<Supervisor> GetAll();
        Task<bool> IsSupervisorOfTeamAsync(long supervisorId, long teamId);
    }
}