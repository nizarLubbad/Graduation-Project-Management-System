using GPMS.DTOS.Project;
using GPMS.Interfaces;
using GPMS.Models;


namespace GPMS.Services
{
    public class TeamService
    {

        private readonly IStudentRepository _studentRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly AppDbContext _dbContext; 

        public TeamService(
            IStudentRepository studentRepository,
            ITeamRepository teamRepository,
            ISupervisorRepository supervisorRepository,
            AppDbContext dbContext)
        {
            _studentRepository = studentRepository;
            _teamRepository = teamRepository;
            _supervisorRepository = supervisorRepository;
            _dbContext = dbContext;
        }
        public async Task<bool> CreateTeamAsync(long creatorId, CreateTeamDto createTeamDto, long supervisorId)
        {
            
            var allStudents = await _studentRepository.GetAllAsync();
            var creator = allStudents.FirstOrDefault(s => s.StudentId == creatorId);

            if (creator == null || creator.Status)
                return false; 

            //select only the students with status false
            var selectedStudents = allStudents
                .Where(s => createTeamDto.MemberStudentIds.Contains(s.StudentId) && !s.Status)
                .ToList();

            
            creator.Status = true;
            creator.TeamId = creatorId;
            await _studentRepository.UpdateAsync(creator);

            foreach (var student in selectedStudents)
            {
                student.Status = true;
                student.TeamId = creatorId;
                await _studentRepository.UpdateAsync(student);
            }

            // create the team
            var team = new Team
            {
                TeamId = creatorId,
                TeamName = createTeamDto.TeamName,
                CreatedDate = DateTime.Now,
                //ProjectTitle = createTeamDto.Project?.ProjectTitle ?? "",
                //SupervisorName = (await _supervisorRepository.GetByIdAsync(supervisorId))?.Name ?? ""
            };

            await _teamRepository.AddAsync(team);

            // update the number of teams that the supervisor can have 
            var supervisor = await _supervisorRepository.GetByIdAsync(supervisorId);
            if (supervisor != null)
            {
                supervisor.TeamCount--;
                await _supervisorRepository.UpdateAsync(supervisor);
            }

            return true;
        }

    }
}
