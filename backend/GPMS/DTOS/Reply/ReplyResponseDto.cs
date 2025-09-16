namespace GPMS.DTOS.Reply
{
    public class ReplyResponseDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public long? StudentId { get; set; }
        public long? SupervisorId { get; set; }
    }
}
