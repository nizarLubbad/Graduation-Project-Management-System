using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.DTOS.Student;
using GPMS.DTOS.Team;
using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;


namespace GPMS.Services
{
    //public class TeamService
    //{

    //    private readonly IStudentRepository _studentRepository;
    //    private readonly ITeamRepository _teamRepository;
    //    private readonly ISupervisorRepository _supervisorRepository;
    //    private readonly AppDbContext _dbContext; 

    //    public TeamService(
    //        IStudentRepository studentRepository,
    //        ITeamRepository teamRepository,
    //        ISupervisorRepository supervisorRepository,
    //        AppDbContext dbContext)
    //    {
    //        _studentRepository = studentRepository;
    //        _teamRepository = teamRepository;
    //        _supervisorRepository = supervisorRepository;
    //        _dbContext = dbContext;
    //    }
    //    public async Task<bool> CreateTeamAsync(long creatorId, CreateTeamDto createTeamDto, long supervisorId)
    //    {

    //        var allStudents = await _studentRepository.GetAllAsync();
    //        var creator = allStudents.FirstOrDefault(s => s.StudentId == creatorId);

    //        if (creator == null || creator.Status)
    //            return false; 

    //        //select only the students with status false
    //        var selectedStudents = allStudents
    //            .Where(s => createTeamDto.MemberStudentIds.Contains(s.StudentId) && !s.Status)
    //            .ToList();


    //        creator.Status = true;
    //        creator.TeamId = creatorId;
    //        await _studentRepository.UpdateAsync(creator);

    //        foreach (var student in selectedStudents)
    //        {
    //            student.Status = true;
    //            student.TeamId = creatorId;
    //            await _studentRepository.UpdateAsync(student);
    //        }

    //        // create the team
    //        var team = new Team
    //        {
    //            TeamId = creatorId,
    //            TeamName = createTeamDto.TeamName,
    //            CreatedDate = DateTime.Now,
    //            //ProjectTitle = createTeamDto.Project?.ProjectTitle ?? "",
    //            //SupervisorName = (await _supervisorRepository.GetByIdAsync(supervisorId))?.Name ?? ""
    //        };

    //        await _teamRepository.AddAsync(team);

    //        // update the number of teams that the supervisor can have 
    //        var supervisor = await _supervisorRepository.GetByIdAsync(supervisorId);
    //        if (supervisor != null)
    //        {
    //            supervisor.TeamCount--;
    //            await _supervisorRepository.UpdateAsync(supervisor);
    //        }

    //        return true;
    //    }
    //public class TeamService : ITeamService
    //{
    //    private readonly ITeamRepository _teamRepository;

    //    public TeamService(ITeamRepository teamRepository)
    //    {
    //        _teamRepository = teamRepository;
    //    }

    //    public async Task<TeamDto?> GetByIdAsync(long teamId)
    //    {
    //        var team = await _teamRepository.GetTeamWithDetailsAsync(teamId);
    //        if (team == null) return null;

    //        return new TeamDto
    //        {
    //            TeamId = team.TeamId,
    //            TeamName = team.TeamName,
    //            TeamStatus = team.Students.Any(s => s.Status),
    //            CreatedDate = team.CreatedDate,
    //            CreatorName = team.Students.FirstOrDefault()?.Name ?? "Unknown",
    //            MemberCount = team.Students.Count,
    //            ProjectTitle = team.Project.ProjectTitle,
    //            SupervisorName = team.Supervisor?.Name ?? "Not Assigned"
    //        };
    //    }

    //    public async Task<TeamDto> CreateAsync(long creatorStudentId, IEnumerable<long> memberStudentIds, string teamName)
    //    {
    //        var team = await _teamRepository.CreateTeamWithStudentsAsync(creatorStudentId, memberStudentIds, teamName);

    //        return new TeamDto
    //        {
    //            TeamId = team.TeamId,
    //            TeamName = team.TeamName,
    //            TeamStatus = true,
    //            CreatedDate = team.CreatedDate,
    //            CreatorName = "N/A", 
    //            MemberCount = memberStudentIds.Count() + 1,
    //            ProjectTitle = team.Project.ProjectTitle ,
    //            SupervisorName = team.Supervisor?.Name ?? "Not Assigned"
    //        };
    //    }

    //    public async Task<bool> DeleteAsync(long teamId)
    //    {
    //        return await _teamRepository.DeleteAsync(teamId);
    //    }

    //    public async Task<IEnumerable<TeamDto>> GetAllAsync()
    //    {
    //        var teams = await _teamRepository.GetAllAsync();

    //        var teamDtos = teams.Select(team => new TeamDto
    //        {
    //            TeamId = team.TeamId,
    //            TeamName = team.TeamName,
    //            TeamStatus = team.Students.Any(s => s.Status),
    //            CreatedDate = team.CreatedDate,
    //            CreatorName = team.Students.FirstOrDefault()?.Name ?? "Unknown",
    //            MemberCount = team.Students.Count,
    //            ProjectTitle = team.Project.ProjectTitle,
    //            SupervisorName = team.Supervisor?.Name ?? "Not Assigned"
    //        });

    //        return teamDtos;
    //    }
    //}
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public TeamService(ITeamRepository teamRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        public async Task<TeamDto?> GetByIdAsync(long teamId)
        {
            var team = await _teamRepository.GetTeamWithDetailsAsync(teamId);
            if (team == null) return null;

            return _mapper.Map<TeamDto>(team);
        }

        public async Task<TeamDto> CreateAsync(long creatorStudentId, IEnumerable<long> memberStudentIds, string teamName)
        {
            var team = await _teamRepository.CreateTeamWithStudentsAsync(creatorStudentId, memberStudentIds, teamName);
            return _mapper.Map<TeamDto>(team);
        }

        public async Task<bool> DeleteAsync(long teamId)
        {
            return await _teamRepository.DeleteAsync(teamId);
        }

        public async Task<IEnumerable<TeamDto>> GetAllAsync()
        {
            var teams = await _teamRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TeamDto>>(teams);
        }
    }


}
