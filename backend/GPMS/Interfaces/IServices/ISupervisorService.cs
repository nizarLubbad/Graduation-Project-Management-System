using GPMS.DTOS.Project;


namespace GPMS.Interfaces
{
    public interface ISupervisorService : IBaseService<SupervisorDto>
    {
        Task<SupervisorDto?> GetByEmailAsync(string email);
    }
}
