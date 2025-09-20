using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface ISupervisorRepository : IBaseRepository<Supervisor>
    {
        //Task<Supervisor?> GetByEmailAsync(string email);
        //Task SaveChangesAsync();
        Task<bool> ExistsByEmailAsync(string email);
        Task<Supervisor?> GetByUserIdAsync(long userId);

    }
}