using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.Models;

namespace GPMS.MappingProfiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, CreateProjectDto>();
            CreateMap<Project, UpdateProjectDto>();
            CreateMap<Project, ProjectResponseDto>();
            CreateMap<Project, UpdateProjectStatusDto>();
        }
    }
}
