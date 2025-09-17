namespace GPMS.DTOS.Reply
{
    public class ReplyResponseDto
    {
        public long Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Date { get; set; }

        public long? StudentId { get; set; }
        public string? StudentName { get; set; }

        public long? SupervisorId { get; set; }
        public string? SupervisorName { get; set; }
    }
}
