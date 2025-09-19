using AutoMapper;
using GPMS.DTOS.Feedback;
using GPMS.DTOS.Reply;
using GPMS.Models;

namespace GPMS.MappingProfiles
{
    public class ReplyProfile : Profile
    {
        public ReplyProfile()
        {
            
            CreateMap<Reply, ReplyResponseDto>();
            CreateMap<Reply, CreateReplyDto>();
            CreateMap<Reply, UpdateReplyDto>();
        }
    }
}
