namespace GPMS.DTOS.Reply
{
    public class CreateReplyDto
    {
        public int FeedbackId { get; set; }
        public string Message { get; set; }
        public long? StudentId { get; set; }
        public long? SupervisorId { get; set; }
    }
}
