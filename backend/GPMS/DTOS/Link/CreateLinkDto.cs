namespace GPMS.DTOS.Link
{
    public class CreateLinkDto
    {
        public string Url { get; set; } = null!;
        public int TaskId { get; set; }
    }
}
