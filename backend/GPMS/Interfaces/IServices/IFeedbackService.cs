using GPMS.DTOS.Feedback;


namespace GPMS.Interfaces
{
    public interface IFeedbackService : IBaseService<FeedbackDto>
    {
        Task<IEnumerable<FeedbackDto>> GetByTeamIdAsync(long teamId);
    }
}
