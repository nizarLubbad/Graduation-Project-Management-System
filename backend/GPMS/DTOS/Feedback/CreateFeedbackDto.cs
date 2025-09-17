namespace GPMS.DTOS.Feedback
{
    public class CreateFeedbackDto
    {
        public string Content { get; set; }
        public long TeamId { get; set; }
        public long SupervisorId { get; set; }
    }
}
