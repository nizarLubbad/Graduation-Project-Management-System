namespace GPMS.DTOS.Reply
{
    public class UpdateReplyDto
    {
        public string Content { get; set; } = null!;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
