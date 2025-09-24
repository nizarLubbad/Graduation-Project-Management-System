
using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.DTOS.Supervisor;
using GPMS.Interfaces;
using GPMS.Models;
using GPMS.Repositories;
using Microsoft.EntityFrameworkCore;

public class SupervisorService : ISupervisorService
{
    private readonly ISupervisorRepository _repository;
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    public SupervisorService(ISupervisorRepository repository, IMapper mapper, ITeamRepository teamRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _teamRepository = teamRepository;
    }

    //public async Task<IEnumerable<SupervisorDto>> GetAllAsync()
    //{
    //    var supervisors = await _repository.GetAllAsync();
    //    return _mapper.Map<IEnumerable<SupervisorDto>>(supervisors);
    //}

    public async Task<bool> SetMaxTeamsAsync(long userId, int maxTeams)
    {
        var supervisor = await _repository.GetByUserIdAsync(userId);
        if (supervisor == null) return false;

        supervisor.MaxTeams = maxTeams;
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IncrementTeamCountAsync(long userId)
    {
        var supervisor = await _repository.GetByUserIdAsync(userId);
        if (supervisor == null) return false;

        if (supervisor.Teams.Count >= supervisor.MaxTeams)
            return false;

        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DecrementTeamCountAsync(long userId)
    {
        var supervisor = await _repository.GetByUserIdAsync(userId);
        if (supervisor == null) return false;

        if (supervisor.Teams.Count == 0)
            return false;

        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<List<SupervisorDto>> GetAllSupervisorsAsync()
    {
        var supervisors = await _repository.GetAll()
            .Include(s => s.User)   // للتأكد من جلب بيانات الـ User المرتبطة
            .Include(s => s.Teams)  // لجلب قائمة الفرق المرتبطة إذا احتجنا
            .ToListAsync();

        var supervisorDtos = supervisors.Select(s => new SupervisorDto
        {
            SupervisorId = s.UserId,
            Name = s.User?.Name ?? "N/A",    // حماية من null
            Email = s.User?.Email ?? "N/A",  // حماية من null
            //Department = s.Department ?? string.Empty,
            TeamCount = s.TeamCount,
            MaxTeams = s.MaxTeams
        }).ToList();

        return supervisorDtos;
    }


    //public async Task<List<SupervisorProfileDto>> GetAllSupervisorProfilesAsync()
    //{
    //    var supervisors = await _repository.GetAll()
    //        .Include(s => s.User)
    //        .Include(s => s.Teams)
    //        .ToListAsync();

    //    return supervisors.Select(supervisor => new SupervisorProfileDto
    //    {
    //        UserId = supervisor.UserId,
    //        Name = supervisor.User.Name,
    //        Email = supervisor.User.Email,
    //        Department = supervisor.Department,
    //        TeamCount = supervisor.TeamCount,
    //        MaxTeams = supervisor.MaxTeams
    //    }).ToList();
    //}
    public async Task<(bool Success, string ErrorMessage)> BookTeamAsync(long supervisorId, long teamId)
    {
        var supervisor = await _repository.GetByIdAsync(supervisorId);
        if (supervisor == null)
            return (false, "Supervisor not found");

        if (supervisor.TeamCount >= supervisor.MaxTeams)
            return (false, "Supervisor has reached the maximum number of teams");

        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            return (false, "Team not found");

        team.SupervisorId = supervisorId;
        supervisor.TeamCount++;

        await _teamRepository.UpdateAsync(team);
        await _repository.UpdateAsync(supervisor);

        return (true, string.Empty);
    }
    public async Task<int?> GetRemainingTeamsAsync(long userId)
    {
        var supervisor = await _repository.GetByUserIdAsync(userId);
        if (supervisor == null) return null;

        return supervisor.MaxTeams - supervisor.TeamCount;
    }

    public async Task<List<SupervisorProfileDto>> GetAllSupervisorProfilesAsync()
    {
        var supervisors = await _repository.GetAll()
            .Include(s => s.User)
            .Include(s => s.Teams)
            .ToListAsync();

        var supervisorDtos = _mapper.Map<List<SupervisorProfileDto>>(supervisors);

        return supervisorDtos;
    }
    public async Task<int?> GetMaxTeamsAsync(long userId)
    {
        var supervisor = await _repository.GetByUserIdAsync(userId);
        if (supervisor == null) return null;

        return supervisor.MaxTeams;
    }






    //public Task<SupervisorProfileDto?> GetSupervisorProfileAsync(long userId)
    //{
    //    throw new NotImplementedException();
    //}
}


