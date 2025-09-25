using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.DTOS.Student;
using GPMS.DTOS.Team;
using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;


namespace GPMS.Services
{
    
    
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository; 
        private readonly ILogger<TeamService> _logger;

        public TeamService(ITeamRepository teamRepository, IMapper mapper, IUserRepository userRepository, ILogger<TeamService> logger)
        {
            _teamRepository = teamRepository;

            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<TeamDto?> GetByIdAsync(long teamId)
        {
            var team = await _teamRepository.GetTeamWithDetailsAsync(teamId);
            if (team == null) return null;

            var dto = _mapper.Map<TeamDto>(team) ?? new TeamDto();

            dto.TeamId = team.TeamId;
            dto.MemberStudentIds = team.Students?.Select(s => s.UserId).ToList() ?? new List<long>();
            dto.TeamName = team.TeamName;
            //dto.CreatedDate = team.CreatedDate;
            dto.MemberCount = team.Students?.Count ?? 0; // Fix: Ensure MemberCount is correctly set.
            dto.SupervisorName = team.Supervisor?.User?.Name;
            dto.SupervisorId = team.Supervisor?.UserId ?? 0;
            if (team.Project != null)
            {
                dto.Project = new CreateProjectDto
                {
                    ProjectTitle = team.Project.ProjectTitle,
                    Description = team.Project.Description,
                    TeamId = team.Project?.TeamId ?? 0,
                    //StartDate = team.Project.StartDate,
                    //EndDate = team.Project.EndDate
                };
            }

            return dto;
        }


        //}
        public async Task<TeamDto> CreateTeamAsync(string teamName, IEnumerable<long> memberStudentIds)
        {
            var students = await _userRepository.GetStudentsByIdsAsync(memberStudentIds.ToList());

            var alreadyInTeam = students.Where(s => s.TeamId != null).ToList();
            if (alreadyInTeam.Any())
            {
                var studentIds = string.Join(", ", alreadyInTeam.Select(s => s.UserId));
                throw new InvalidOperationException($"This student has team so you can not added: {studentIds}");
            }

            var team = new Team
            {
                TeamName = teamName,
                CreatedDate = DateTime.Now
            };

            foreach (var student in students)
            {
                student.TeamId = team.TeamId;
                team.Students.Add(student);
            }

            await _teamRepository.AddAsync(team);
            await _teamRepository.SaveChangesAsync();

            var teamDto = _mapper.Map<TeamDto>(team);

            teamDto.MemberCount = team.Students.Count;
            teamDto.MemberStudentIds = team.Students.Select(s => s.UserId).ToList();

            if (team.Project != null)
            {
                teamDto.ProjectTitle = team.Project.ProjectTitle;
            }

            if (team.Supervisor != null)
            {
                teamDto.SupervisorName = team.Supervisor.User?.Name;
            }

            return teamDto;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //public async Task<TeamDto> CreateTeamAsync(string teamName, IEnumerable<long> memberStudentIds)
        //{
        //    var team = new Team
        //    {
        //        TeamName = teamName,
        //        CreatedDate = DateTime.Now
        //    };

        //    var students = await _userRepository.GetStudentsByIdsAsync(memberStudentIds.ToList());

        //    foreach (var student in students)
        //    {
        //        student.TeamId = team.TeamId;
        //        team.Students.Add(student);
        //    }

        //    await _teamRepository.AddAsync(team);
        //    await _teamRepository.SaveChangesAsync();

        //    var teamDto = _mapper.Map<TeamDto>(team);
        //    teamDto.MemberCount = team.Students.Count;
        //    teamDto.MemberStudentIds = team.Students?.Select(s => s.UserId).ToList();

        //    return teamDto;


        //}

        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //public async Task<TeamDto> CreateTeamAsync(string teamName, IEnumerable<long> memberStudentIds)
        //{
        //    var team = new Team
        //    {
        //        TeamName = teamName,
        //        CreatedDate = DateTime.Now
        //    };

        //    var students = await _userRepository.GetStudentsByIdsAsync(memberStudentIds.ToList());

        //    foreach (var student in students)
        //    {
        //        student.TeamId = team.TeamId;
        //        student.Status = true;
        //        team.Students.Add(student);
        //    }

        //    await _teamRepository.AddAsync(team);
        //    await _teamRepository.SaveChangesAsync();

        //    var teamDto = _mapper.Map<TeamDto>(team);

        //    teamDto.MemberCount = team.Students.Count;
        //    teamDto.MemberStudentIds = team.Students?.Select(s => s.UserId).ToList();

        //    return teamDto;
        //}
        public async Task<bool> DeleteAsync(long teamId)
        {
            return await _teamRepository.DeleteAsync(teamId);
        }

        //public async Task<IEnumerable<TeamDto>> GetAllAsync()
        //{
        //    var teams = await _teamRepository.GetAllAsync();
        //    var teamDtos = new List<TeamDto>();

        //    foreach (var team in teams)
        //    {
        //        var teamDto = _mapper.Map<TeamDto>(team);
        //        teamDto.MemberCount = team.Students?.Count ?? 0;
        //        teamDto.MemberStudentIds = team.Students?.Select(s => s.UserId).ToList() ?? new List<long>();
        //        teamDto.SupervisorName = team.Supervisor?.User?.Name;
        //        teamDto.ProjectTitle = team.Project?.ProjectTitle;
        //        teamDtos.Add(teamDto);
        //    }

        //    return teamDtos;
        //}
        public async Task<IEnumerable<TeamDto>> GetAllAsync()
        {
            var teams = await _teamRepository.GetAllAsync();
            var teamDtos = new List<TeamDto>();

            foreach (var team in teams)
            {
                var teamDto = _mapper.Map<TeamDto>(team);

                teamDto.MemberCount = team.Students?.Count ?? 0;
                teamDto.MemberStudentIds = team.Students?.Select(s => s.UserId).ToList() ?? new List<long>();

                teamDto.ProjectTitle = team.Project?.ProjectTitle;
                teamDto.SupervisorName = team.Supervisor?.User?.Name;
                teamDto.SupervisorId = team.Supervisor?.UserId ?? 0;


                teamDtos.Add(teamDto);
            }

            return teamDtos;
        }
        public async Task UpdateStudentsStatusAsync(long teamId)
        {
            var students = await _teamRepository.GetTeamStudentsAsync(teamId);
            if (students == null)
            {
                return;
            }

            foreach (var student in students)
            {
                student.Status = true;
            }

            await _teamRepository.SaveChangesAsync();
        }
        public async Task<List<StudentDto>> GetTeamMembersByStudentIdAsync(long studentId)
        {
            _logger.LogInformation("Fetching team members for studentId: {StudentId}", studentId);

            var members = await _teamRepository.GetTeamMembersByStudentIdAsync(studentId);

            _logger.LogInformation("Found {Count} members in the team for studentId: {StudentId}", members.Count(), studentId);

            var result = _mapper.Map<List<StudentDto>>(members);

            return result;
        }


    }


}
