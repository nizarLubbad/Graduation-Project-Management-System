namespace GPMS.DTOS.Student
{
    public class StudentProfileDto
    {
        public long UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool? Status { get; set; } = false;
        public string? Department { get; set; }
        public long? TeamId { get; set; }
        public string? TeamName { get; set; }
    }
}
