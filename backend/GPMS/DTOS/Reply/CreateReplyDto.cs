namespace GPMS.DTOS.Reply
{
    public class CreateReplyDto
    {
        public string Content { get; set; } = null!;
        public long FeedbackId { get; set; }
        public long? StudentId { get; set; }
        public long? SupervisorId { get; set; }
    }
}
