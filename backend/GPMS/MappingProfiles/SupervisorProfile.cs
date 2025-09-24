using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.DTOS.Supervisor;
using GPMS.Models;

namespace GPMS.MappingProfiles
{
    public class SupervisorProfile : Profile
    {
        public SupervisorProfile()
        {
            CreateMap<Supervisor,CreateSupervisorDto>();
            CreateMap<Supervisor,SupervisorDto>();
            CreateMap<Supervisor,SetMaxTeamsDto>();
        }
    }
}
