using AutoMapper;
using GPMS.DTOS.Team;
using GPMS.Models;

namespace GPMS.MappingProfiles
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<Team,CreateTeamDto>();
            CreateMap<Team,TeamDto>();
            CreateMap<Team,UpdateTeamDto>();
        }
    }
}
