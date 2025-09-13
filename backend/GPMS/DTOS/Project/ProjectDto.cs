namespace GPMS.DTOS.Project
{
    public class ProjectDto
    {
        //public string ProjectTitle { get; set; } // PK
        //public string Description { get; set; }
        public bool ProjectStatus { get; set; }
        public DateTime CreatedDate { get; set; }

        public long TeamId { get; set; }
        //public string TeamName { get; set; }
        //public string SupervisorName { get; set; }
    }
}
