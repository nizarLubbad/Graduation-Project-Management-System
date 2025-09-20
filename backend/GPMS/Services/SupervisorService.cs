using GPMS.DTOS.Project;
using GPMS.Interfaces;

namespace GPMS.Services
{
    public class SupervisorService : ISupervisorService
    {
        private readonly ISupervisorRepository _supervisorRepository;

        public SupervisorService(ISupervisorRepository supervisorRepository)
        {
            _supervisorRepository = supervisorRepository;
        }

        public Task<SupervisorDto> CreateAsync(SupervisorDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SupervisorDto>> GetAllAsync()
        {
            var supervisors = await _supervisorRepository.GetAllAsync();

            return supervisors.Select(s => new SupervisorDto
            {
                SupervisorId = s.SupervisorId,        
                Name = s.Name,
                Email = s.Email,
                TeamCount = s.Teams?.Count ?? 0
            });
        }

        public async Task<SupervisorDto?> GetByIdAsync(long id)
        {
            var supervisor = await _supervisorRepository.GetByIdAsync(id);
            if (supervisor == null) return null;

            return new SupervisorDto
            {
                SupervisorId = supervisor.SupervisorId,
                Name = supervisor.Name,
                Email = supervisor.Email,
                TeamCount = supervisor.Teams?.Count ?? 0
            };
        }

        public Task<SupervisorDto?> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<SupervisorDto?> UpdateAsync(int id, SupervisorDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<SupervisorDto?> UpdateAsync(object id, SupervisorDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
