
using GPMS.DTOS.Project;

namespace GPMS.DTOS.Team
{
    public class CreateTeamDto
    {
        public string TeamName { get; set; }

        // Optional: list of student ids to add as members when creating
        public List<long> MemberStudentIds { get; set; } = new List<long>();

        // Optional: create project with the team in the same component
        public CreateProjectDto Project { get; set; }
    }
}
