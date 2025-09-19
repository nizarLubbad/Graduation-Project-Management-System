using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.Models;

namespace GPMS.MappingProfiles
{
    public class SupervisorProfile : Profile
    {
        public SupervisorProfile()
        {
            CreateMap<Supervisor,CreateSupervisorDto>();
            CreateMap<Supervisor,SupervisorDto>();
        }
    }
}
