using AutoMapper;
using GPMS.DTOS.Feedback;
using GPMS.Models;

namespace GPMS.MappingProfiles
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, CreateFeedbackDto>();
            CreateMap<Feedback,FeedbackDto>();
            CreateMap<Feedback, FeedbackResponseDto>();
            CreateMap<Feedback,UpdateFeedbackDto>();
        }
    }
}
