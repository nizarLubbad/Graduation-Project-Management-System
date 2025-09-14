namespace GPMS.DTOS.Feedback
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Date { get; set; }
        //public int TaskId { get; set; }
    }
}
