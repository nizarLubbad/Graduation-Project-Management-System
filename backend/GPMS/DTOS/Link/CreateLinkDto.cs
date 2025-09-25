namespace GPMS.DTOS.Link
{
    public class CreateLinkDto
    {
        public string Url { get; set; } = null!;
        public string Title { get; set; } = null!;
        public long StudentId { get; set; }
        //public long TeamId { get; set; }
    }
}
