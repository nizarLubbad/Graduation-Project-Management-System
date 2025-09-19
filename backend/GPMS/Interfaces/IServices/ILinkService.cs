using GPMS.DTOS.Link;

namespace GPMS.Interfaces
{
    public interface ILinkService 
    {
        Task<LinkDto?> GetByIdAsync(long id);
        Task<bool> DeleteAsync(long id);
        //GetAllAsync() 
        Task<IEnumerable<LinkDto>> GetAllAsync();

        Task<IEnumerable<LinkDto>> GetByTeamIdAsync(long teamId);

        Task<IEnumerable<LinkDto>> GetByStudentIdAsync(long studentId);
        Task<LinkDto> CreateAsync(CreateLinkDto dto);
        Task<LinkDto?> UpdateAsync(long id, LinkDto dto);

    }
}
