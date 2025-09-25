namespace GPMS.DTOS.Supervisor
{
    public class SupervisorProfileDto
    {
        public long UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Department { get; set; }
        public int TeamCount { get; set; }
        public int MaxTeams { get; set; }
    }
}
