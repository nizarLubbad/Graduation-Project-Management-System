using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface ISupervisorRepository : IBaseRepository<Supervisor>
    {
        Task<Supervisor> GetByEmailAsync(string email);
    }
}