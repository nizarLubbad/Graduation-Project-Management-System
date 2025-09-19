namespace GPMS.DTOS.Task
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long TeamId { get; set; } // team where the task belongs
    }
}
