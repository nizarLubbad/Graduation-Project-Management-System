namespace GPMS.DTOS.Link
{
    public class LinkDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public long TeamId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
