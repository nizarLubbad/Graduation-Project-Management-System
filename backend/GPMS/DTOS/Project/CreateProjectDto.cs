namespace GPMS.DTOS.Project
{
    public class CreateProjectDto
    {
        public string ProjectTitle { get; set; } = null!;
        public string Description { get; set; } = null!;
        //public long SupervisorId { get; set; }
        public long TeamId { get; set; }
        //public bool IsCompleted { get; set; } = false;

    }
}
