using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.Interfaces;
using GPMS.Models;

namespace GPMS.Services
{
    public class SupervisorService : ISupervisorService
    {
        public Task<SupervisorDto> CreateAsync(SupervisorDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SupervisorDto>> GetAllAsync()
        {
            throw new NotImplementedException();
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
    //    {
    //        private readonly ISupervisorRepository _repository;
    //        private readonly IMapper _mapper;

    //        public SupervisorService(ISupervisorRepository repository, IMapper mapper)
    //        {
    //            _repository = repository;
    //            _mapper = mapper;
    //        }


    //        public async Task<SupervisorDto> CreateAsync(SupervisorDto dto)
    //        {
    //            var supervisor = _mapper.Map<Supervisor>(dto);
    //            await _repository.AddAsync(supervisor);
    //            await _repository.SaveChangesAsync();
    //            return _mapper.Map<SupervisorDto>(supervisor);
    //        }


    //        public async Task<IEnumerable<SupervisorDto>> GetAllAsync()
    //        {
    //            var supervisors = await _repository.GetAllAsync();
    //            return _mapper.Map<IEnumerable<SupervisorDto>>(supervisors);
    //        }


    //        public async Task<SupervisorDto?> GetByEmailAsync(string email)
    //        {
    //            var supervisor = await _repository.GetByEmailAsync(email);
    //            return _mapper.Map<SupervisorDto?>(supervisor);
    //        }


    //        public async Task<SupervisorDto?> GetByIdAsync(long supervisorId)
    //        {
    //            var supervisor = await _repository.GetByIdAsync(supervisorId);
    //            return _mapper.Map<SupervisorDto?>(supervisor);
    //        }


    //        public async Task<SupervisorDto?> GetByIdAsync(object id)
    //        {
    //            var supervisorId = Convert.ToInt64(id);
    //            return await GetByIdAsync(supervisorId);
    //        }

    //        public async Task<SupervisorDto?> UpdateAsync(long supervisorId, SupervisorDto dto)
    //        {
    //            var supervisor = await _repository.GetByIdAsync(supervisorId);
    //            if (supervisor == null) return null;

    //            supervisor.Name = dto.Name;
    //            supervisor.Email = dto.Email;

    //            _repository.Update(supervisor);
    //            await _repository.SaveChangesAsync();

    //            return _mapper.Map<SupervisorDto>(supervisor);
    //        }


    //        public async Task<SupervisorDto?> UpdateAsync(object id, SupervisorDto dto)
    //        {
    //            var supervisorId = Convert.ToInt64(id);
    //            return await UpdateAsync(supervisorId, dto);
    //        }


    //        public async Task<bool> DeleteAsync(long supervisorId)
    //        {
    //            var supervisor = await _repository.GetByIdAsync(supervisorId);
    //            if (supervisor == null) return false;

    //            _repository.Delete(supervisor);
    //            await _repository.SaveChangesAsync();
    //            return true;
    //        }


    //        public async Task<bool> DeleteAsync(object id)
    //        {
    //            var supervisorId = Convert.ToInt64(id);
    //            return await DeleteAsync(supervisorId);
    //        }

