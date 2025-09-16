using static GPMS.Models.Enums;

namespace GPMS.DTOS.KanbanTask
{
    public class CreateTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public PriorityLevel Priority { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
