namespace GPMS.DTOS.Team
{
    public class UpdateTeamDto
    {
        public string TeamName { get; set; } = null!;
        public long? SupervisorId { get; set; }
    }
}
