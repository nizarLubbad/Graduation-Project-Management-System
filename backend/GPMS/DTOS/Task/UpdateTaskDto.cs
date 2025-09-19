using GPMS.Models.Enums;

namespace GPMS.DTOS.Task
{
    public class UpdateTaskDto
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskPriority? Priority { get; set; }
        public long? TeamId { get; set; }

        public IEnumerable<string>? AssignedStudentNames { get; set; }

    }
}
