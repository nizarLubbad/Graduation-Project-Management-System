using GPMS.DTOS.Project;


namespace GPMS.Interfaces
{
    public interface ISupervisorService : 
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(int id);
        Task<TDto> CreateAsync(TDto dto);
        Task<TDto?> UpdateAsync(int id, TDto dto);
        Task<bool> DeleteAsync(int id);
        Task<SupervisorDto?> GetByEmailAsync(string email);
    }
}
