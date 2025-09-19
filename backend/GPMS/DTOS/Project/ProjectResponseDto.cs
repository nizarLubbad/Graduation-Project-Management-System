namespace GPMS.DTOS.Project
{
    public class ProjectResponseDto
    {
        //public int Id { get; set; }
        //public string ProjectName { get; set; } = string.Empty;
        //public string Description { get; set; } = string.Empty;
        //public bool IsCompleted { get; set; }

        //// Supervisor info
        //public int SupervisorId { get; set; }
        //public string SupervisorName { get; set; } = string.Empty;

        //// List of student names
        //public List<string> StudentNames { get; set; } = new List<string>();
        public int Id { get; set; }
        public string ProjectName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;

        public long SupervisorId { get; set; }
        public string SupervisorName { get; set; } = null!;

        public long TeamId { get; set; }
        public string TeamName { get; set; } = null!;
    }
}
