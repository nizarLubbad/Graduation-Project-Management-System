using AutoMapper;
using GPMS.DTOS.Link;
using GPMS.DTOS.Reply;
using GPMS.Models;

namespace GPMS.MappingProfiles
{
    public class LinkProfile :Profile
    {
        public LinkProfile()
        {
            CreateMap<Link,CreateLinkDto>();
            CreateMap<Link,LinkDto>();
        }

    }
}
