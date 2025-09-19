using GPMS.DTOS.Feedback;


namespace GPMS.Interfaces
{
    public interface IFeedbackService 
    {

        Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto feedbackCreateDto);
        //Task<FeedbackDto> AddFeedbackAsync(CreateFeedbackDto feedbackCreateDto);
        Task<FeedbackDto?> UpdateFeedbackAsync(long feedbackId, UpdateFeedbackDto dto);
        Task<bool> DeleteFeedbackAsync(long feedbackId);
        Task<FeedbackDto?> GetByIdAsync(long feedbackId);


        Task<IEnumerable<FeedbackDto>> GetByTeamIdAsync(long teamId);
    }
}
