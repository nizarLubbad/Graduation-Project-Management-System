namespace GPMS.DTOS.Project
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }

        // Supervisor info
        public int SupervisorId { get; set; }
        public string SupervisorName { get; set; } = string.Empty;

        // List of student names
        public List<string> StudentNames { get; set; } = new List<string>();
    }
}
