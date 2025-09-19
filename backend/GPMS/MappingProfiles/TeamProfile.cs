using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.Models;

namespace GPMS.MappingProfiles
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<Team,CreateTeamDto>();
        }
    }
}
