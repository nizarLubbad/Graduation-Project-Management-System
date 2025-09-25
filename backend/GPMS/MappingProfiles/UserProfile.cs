using AutoMapper;
using GPMS.DTOS.Auth;
using GPMS.Models;

namespace GPMS.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
