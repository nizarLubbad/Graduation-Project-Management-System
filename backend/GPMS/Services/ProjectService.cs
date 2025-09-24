using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Services
{
    

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
            var response = _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);

            foreach (var (dto, project) in response.Zip(projects))
            {
                dto.SupervisorId = project.Team?.Supervisor?.UserId;
                dto.SupervisorName = project.Team?.Supervisor?.User?.Name;
                dto.ProjectName = project.ProjectTitle;
                dto.TeamName = project.Team?.TeamName;
            }

            return response;
        }


        public async Task<ProjectResponseDto?> GetByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;

            var response = _mapper.Map<ProjectResponseDto>(project);
            response.SupervisorId = project.Team?.Supervisor?.UserId;
            response.SupervisorName = project.Team?.Supervisor?.User?.Name;

            return response;
        }

        public async Task<ProjectResponseDto?> CreateAsync(CreateProjectDto dto)
        {
            var project = new Project
            {
                ProjectTitle = dto.ProjectTitle,
                Description = dto.Description,
                IsCompleted = false, 
                TeamId = dto.TeamId,

            };

            var team = await _projectRepository.GetTeamWithSupervisorAsync(dto.TeamId);
            if (team == null) return null;

            project.Team = team;

            await _projectRepository.AddAsync(project);

            var response = _mapper.Map<ProjectResponseDto>(project);
            response.SupervisorId = team.Supervisor?.UserId;
            response.SupervisorName = team.Supervisor?.User?.Name;


            return response;
        }


        public async Task<ProjectResponseDto?> UpdateAsync(int projectId, UpdateProjectDto dto)
            {
                var project = await _projectRepository.GetByIdAsync(projectId);
                if (project == null) return null;

                if (!string.IsNullOrEmpty(dto.ProjectName))
                    project.ProjectTitle = dto.ProjectName;

                if (!string.IsNullOrEmpty(dto.Description))
                    project.Description = dto.Description;

                //if (dto.SupervisorId.HasValue)
                //    project.SupervisorId = dto.SupervisorId.Value;

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
                if (project == null) return ;

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
