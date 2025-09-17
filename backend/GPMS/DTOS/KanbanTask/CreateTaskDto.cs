namespace GPMS.DTOS.Task
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public long TeamId { get; set; }
        public List<long> AssignedStudentIds { get; set; } = new();
        public int Priority { get; set; } = 2; // Default Medium
    }
}
