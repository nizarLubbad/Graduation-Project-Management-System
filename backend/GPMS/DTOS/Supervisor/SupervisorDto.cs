namespace GPMS.DTOS.Project
{
    public class SupervisorDto
    {
        public long SupervisorId { get; set; }       // المفتاح الأساسي
        public string Name { get; set; }
        public string Email { get; set; }
        public int TeamCount { get; set; }
        public int MaxTeams { get; set; }

    }
}
