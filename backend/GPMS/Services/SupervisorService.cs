using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.Interfaces;
using GPMS.Models;
using GPMS.Repositories;

namespace GPMS.Services
{
    public class SupervisorService : ISupervisorService
    {
        private readonly ISupervisorRepository _repository;
        private readonly IMapper _mapper;
        public SupervisorService(ISupervisorRepository _repository,IMapper mapper )
        {
            this._repository = _repository;
            mapper = _mapper;

        }


        public async Task<SupervisorDto> CreateAsync(SupervisorDto dto)
        {
            var supervisor = _mapper.Map<Supervisor>(dto);
            await _repository.AddAsync(supervisor);
            return _mapper.Map<SupervisorDto>(supervisor);
        }

        public Task<TDto> CreateAsync(TDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(long Teamid)
        {
            var supervisor = await _repository.GetByIdAsync(Teamid); // it should take a long  , change the ISupervisorRepo to not extends from IBaseRepo abd take own method
            if (supervisor == null) return false;

            await _repository.DeleteAsync(supervisor);
            return true;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SupervisorDto>> GetAllAsync()
        {
            var supervisors = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupervisorDto>>(supervisors);
        }

        public async Task<SupervisorDto?> GetByEmailAsync(string email)
        {
            var supervisor = await _repository.GetByEmailAsync(email);
            return _mapper.Map<SupervisorDto?>(supervisor);
        }

        public async Task<SupervisorDto?> GetByIdAsync(long Teamid)
        {
            var supervisor = await _repository.GetByIdAsync(Teamid);
            return _mapper.Map<SupervisorDto?>(supervisor);
        }

        public Task<TDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<SupervisorDto?> UpdateAsync(long teamid, SupervisorDto dto)
        {
            var supervisor = await _repository.GetByIdAsync(teamid);
            if (supervisor == null) return null;

            // تحديث القيم
            supervisor.Name = dto.Name;
            supervisor.Email = dto.Email;
            supervisor.Department = dto.Department;

            await _repository.UpdateAsync(supervisor);

            return _mapper.Map<SupervisorDto>(supervisor);
        }

        public Task<TDto?> UpdateAsync(int id, TDto dto)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TDto>> ISupervisorService.GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
