namespace GPMS.DTOS.Task
{
    public class UpdateTaskDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public List<long>? AssignedStudentIds { get; set; }
        public Models.Enums.TaskPriority? Priority { get; set; }
        public Models.Enums.TaskStatus? Status { get; set; }
    }
}