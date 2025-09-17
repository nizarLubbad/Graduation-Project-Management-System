namespace GPMS.DTOS.Task
{
    public class TaskResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; } = null!;
        public string Status { get; set; } = null!;
        public long TeamId { get; set; }
        public List<string> AssignedStudents { get; set; } = new();
    }
}
