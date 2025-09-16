namespace GPMS.DTOS.KanbanTask
{
    public class UpdateTaskStatusDto
    {
        public int TaskId { get; set; }
        public string Status { get; set; } = "To Do";
    }
}
