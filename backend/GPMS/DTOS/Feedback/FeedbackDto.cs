namespace GPMS.DTOS.Feedback
{
    public class FeedbackDto
    {
        //public long Id { get; set; }
        //public string Content { get; set; } = null!;
        ////public DateTime Date { get; set; }
        ////public long TeamId { get; set; }
        ////public long SupervisorId { get; set; }
        ///
        public long FeedbackId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Date { get; set; }
        public long TeamId { get; set; }
        public long SupervisorId { get; set; }
    }
}
