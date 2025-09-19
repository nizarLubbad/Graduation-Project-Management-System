using GPMS.DTOS.Project;


namespace GPMS.Interfaces
{
    public interface ISupervisorService : IBaseService<SupervisorDto>
    {
        //Task<IEnumerable<TDto>> GetAllAsync();
        //Task<TDto?> GetByIdAsync(int id);
        Task<SupervisorDto> CreateAsync(SupervisorDto dto);
        Task<SupervisorDto?> UpdateAsync(int id, SupervisorDto dto);
        //Task<bool> DeleteAsync(int id);
        Task<SupervisorDto?> GetByEmailAsync(string email);
    }
}
