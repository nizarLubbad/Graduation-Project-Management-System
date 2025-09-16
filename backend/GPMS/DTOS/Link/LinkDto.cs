namespace GPMS.DTOS.Link
{
    public class LinkDto
    {
        public long Id { get; set; }
        public string Url { get; set; } = null!;
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }

        public long StudentId { get; set; }
        public string StudentName { get; set; } = null!;

        public long TeamId { get; set; }
    }
}
