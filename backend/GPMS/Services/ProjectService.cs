using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.Interfaces;
using GPMS.Models;

namespace GPMS.Services
{
    //public class ProjectService : IProjectService
    //{
    //    private readonly IProjectRepository _repo;
    //    private readonly IMapper _mapper;

    //    public ProjectService(IProjectRepository repo,IMapper mapper)
    //    {
    //        _repo = repo;
    //        _mapper = mapper;
    //    }

    //    public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync()
    //    {
    //        var projects = await _repo.GetAllAsync();
    //        return projects.Select(p => new ProjectResponseDto
    //        {
    //            Id = p.Id,
    //            ProjectName = p.ProjectName,
    //            Description = p.Description,
    //            IsCompleted = p.IsCompleted,
    //            SupervisorId = p.SupervisorId,
    //            SupervisorName = p.Supervisor.Name,
    //            StudentNames = p.Students.Select(s => s.Name).ToList()
    //        });
    //    }

    //    public async Task<ProjectResponseDto?> GetByIdAsync(int id)
    //    {
    //        var project = await _repo.GetByIdAsync(id);
    //        if (project == null) return null;

    //        return new ProjectResponseDto
    //        {
    //            Id = project.Id,
    //            ProjectName = project.ProjectTitle,
    //            Description = project.Description,
    //            IsCompleted = project.IsCompleted,
    //            SupervisorId = project.SupervisorId,
    //            SupervisorName = project.Supervisor.Name,
    //            //Student = project.Students.Select(s => s.Name).ToList()
    //        };
    //    }

    //    public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto)
    //    {
    //        var project = new Project
    //        {
    //            ProjectName = dto.ProjectName,
    //            Description = dto.Description,
    //            SupervisorId = dto.SupervisorId,
    //            IsCompleted = dto.IsCompleted
    //        };

    //        var created = await _repo.AddAsync(project);

    //        return new ProjectResponseDto
    //        {
    //            Id = created.Id,
    //            ProjectName = created.ProjectName,
    //            Description = created.Description,
    //            IsCompleted = created.IsCompleted,
    //            SupervisorId = created.SupervisorId,
    //            SupervisorName = created.Supervisor.Name,
    //            StudentNames = new List<string>()
    //        };
    //    }

    //    public async Task<ProjectResponseDto?> UpdateAsync(int id, UpdateProjectDto dto)
    //    {
    //        var project = await _repo.GetByIdAsync(id);
    //        if (project == null) return null;

    //        project.ProjectName = dto.ProjectName;
    //        project.Description = dto.Description;
    //        project.SupervisorId = dto.SupervisorId;
    //        project.IsCompleted = dto.IsCompleted;

    //        var updated = await _repo.UpdateAsync(project);

    //        return new ProjectResponseDto
    //        {
    //            Id = updated.Id,
    //            ProjectName = updated.ProjectName,
    //            Description = updated.Description,
    //            IsCompleted = updated.IsCompleted,
    //            SupervisorId = updated.SupervisorId,
    //            SupervisorName = updated.Supervisor.Name,
    //            StudentNames = updated.Students.Select(s => s.Name).ToList()
    //        };
    //    }

    //    public async Task<bool> DeleteAsync(int id)
    //    {
    //        return await _repo.DeleteAsync(id);
    //    }

    //    public Task<ProjectResponseDto> CreateAsync(ProjectResponseDto dto)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<ProjectResponseDto?> UpdateAsync(int id, ProjectResponseDto dto)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

        public class ProjectService : IProjectService
        {
            private readonly IProjectRepository _projectRepository;
            private readonly IMapper _mapper;

            public ProjectService(IProjectRepository projectRepository, IMapper mapper)
            {
                _projectRepository = projectRepository;
                _mapper = mapper;
            }

            public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync()
            {
                var projects = await _projectRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
            }

            public async Task<ProjectResponseDto?> GetByIdAsync(int projectId)
            {
                var project = await _projectRepository.GetByIdAsync(projectId);
                if (project == null) return null;
                return _mapper.Map<ProjectResponseDto>(project);
            }

            public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto)
            {
                var project = _mapper.Map<Project>(dto);
                var saved = await _projectRepository.AddAsync(project);
                return _mapper.Map<ProjectResponseDto>(saved);
            }

            public async Task<ProjectResponseDto?> UpdateAsync(int projectId, UpdateProjectDto dto)
            {
                var project = await _projectRepository.GetByIdAsync(projectId);
                if (project == null) return null;

                if (!string.IsNullOrEmpty(dto.ProjectName))
                    project.ProjectTitle = dto.ProjectName;

                if (!string.IsNullOrEmpty(dto.Description))
                    project.Description = dto.Description;

                if (dto.SupervisorId.HasValue)
                    project.SupervisorId = dto.SupervisorId.Value;

                if (dto.TeamId.HasValue)
                    project.TeamId = dto.TeamId.Value;

                var updated = await _projectRepository.UpdateAsync(project);
                return _mapper.Map<ProjectResponseDto>(updated);
            }

            public async Task<bool> DeleteAsync(int projectId)
            {
                return await _projectRepository.DeleteAsync(projectId);
            }

            public async Task UpdateStatusToTrueAsync(int projectId)
            {
                var project = await _projectRepository.GetByIdAsync(projectId);
                if (project == null) return;

                project.IsCompleted = true;
                await _projectRepository.UpdateAsync(project);
            }

            public async Task<bool?> GetStatusAsync(int projectId)
            {
                var project = await _projectRepository.GetByIdAsync(projectId);
                if (project == null) return null;

                return project.IsCompleted;
            }
        }

    }
