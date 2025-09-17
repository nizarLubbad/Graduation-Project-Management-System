namespace GPMS.DTOS.Project
{
    public class UpdateProjectDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SupervisorId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
