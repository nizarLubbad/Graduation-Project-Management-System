namespace GPMS.DTOS.Project
{
    public class UpdateProjectDto
    {
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public long? SupervisorId { get; set; }
        public long? TeamId { get; set; }
    }
}
