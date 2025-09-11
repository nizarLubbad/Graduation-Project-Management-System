namespace GPMS.DTOS.Project
{
    public class TeamDto
    {
        public long TeamId { get; set; }      // equals creator StudentId
        public string TeamName { get; set; }
        public bool TeamStatus { get; set; }
        public DateTime CreatedDate { get; set; }

        // info
        public string CreatorName { get; set; }
        public int MemberCount { get; set; }
        public string ProjectTitle { get; set; }
        public string SupervisorName { get; set; }
    }
}
