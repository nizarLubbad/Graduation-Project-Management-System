using GPMS.DTOS.Project;

namespace GPMS.Interfaces
{
    public interface IProjectService 
    {
        //Task<IEnumerable<ProjectResponseDto>> GetAllAsync();
        //Task<ProjectResponseDto?> GetByIdAsync(object id);
        //Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto);
        //Task<ProjectResponseDto?> UpdateAsync(object id, ProjectResponseDto dto);
        //Task<bool> DeleteAsync(object id);
        Task<IEnumerable<ProjectResponseDto>> GetAllAsync();
        Task<ProjectResponseDto?> GetByIdAsync(int projectId);

        Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto);

        Task<ProjectResponseDto?> UpdateAsync(int projectId, UpdateProjectDto dto);

        Task<bool> DeleteAsync(int projectId);
        // UpdateStatusToTrueAsync
        Task UpdateStatusToTrueAsync(int projectId);
        // GetStatusAsync
        Task<bool?> GetStatusAsync(int projectId);

    }
}
