namespace GPMS.DTOS.Team
{
    public class CreateTeamRequest
    {
        public long CreatorStudentId { get; set; }
        public IEnumerable<long> MemberStudentIds { get; set; } = new List<long>();
        public string TeamName { get; set; } = null!;
    }
}
