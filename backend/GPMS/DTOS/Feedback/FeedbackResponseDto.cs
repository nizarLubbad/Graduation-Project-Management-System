using GPMS.DTOS.Reply;

namespace GPMS.DTOS.Feedback
{
    public class FeedbackResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public long SupervisorId { get; set; }
        public List<ReplyResponseDto> Replies { get; set; }
    }
}
