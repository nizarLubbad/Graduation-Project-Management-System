using GPMS.DTOS.Reply;

namespace GPMS.DTOS.Feedback
{
    public class FeedbackResponseDto
    {
        public long Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Date { get; set; }
        public long SupervisorId { get; set; }
        public string? SupervisorName { get; set; }
        public long TeamId { get; set; }
        public List<ReplyResponseDto> Replies { get; set; } = new();
    }
}
