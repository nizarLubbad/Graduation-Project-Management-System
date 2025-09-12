namespace GPMS.DTOS.KanbanTask
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
