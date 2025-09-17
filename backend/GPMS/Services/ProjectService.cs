using GPMS.DTOS.Project;
using GPMS.Interfaces;
using GPMS.Models;

namespace GPMS.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repo;

        public ProjectService(IProjectRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync()
        {
            var projects = await _repo.GetAllAsync();
            return projects.Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                Description = p.Description,
                IsCompleted = p.IsCompleted,
                SupervisorId = p.SupervisorId,
                SupervisorName = p.Supervisor.Name,
                StudentNames = p.Students.Select(s => s.Name).ToList()
            });
        }

        public async Task<ProjectResponseDto?> GetByIdAsync(int id)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return null;

            return new ProjectResponseDto
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                IsCompleted = project.IsCompleted,
                SupervisorId = project.SupervisorId,
                SupervisorName = project.Supervisor.Name,
                StudentNames = project.Students.Select(s => s.Name).ToList()
            };
        }

        public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto)
        {
            var project = new Project
            {
                ProjectName = dto.ProjectName,
                Description = dto.Description,
                SupervisorId = dto.SupervisorId,
                IsCompleted = dto.IsCompleted
            };

            var created = await _repo.AddAsync(project);

            return new ProjectResponseDto
            {
                Id = created.Id,
                ProjectName = created.ProjectName,
                Description = created.Description,
                IsCompleted = created.IsCompleted,
                SupervisorId = created.SupervisorId,
                SupervisorName = created.Supervisor.Name,
                StudentNames = new List<string>()
            };
        }

        public async Task<ProjectResponseDto?> UpdateAsync(int id, UpdateProjectDto dto)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return null;

            project.ProjectName = dto.ProjectName;
            project.Description = dto.Description;
            project.SupervisorId = dto.SupervisorId;
            project.IsCompleted = dto.IsCompleted;

            var updated = await _repo.UpdateAsync(project);

            return new ProjectResponseDto
            {
                Id = updated.Id,
                ProjectName = updated.ProjectName,
                Description = updated.Description,
                IsCompleted = updated.IsCompleted,
                SupervisorId = updated.SupervisorId,
                SupervisorName = updated.Supervisor.Name,
                StudentNames = updated.Students.Select(s => s.Name).ToList()
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
