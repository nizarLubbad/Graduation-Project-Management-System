using GPMS.Models;
using GPMS.Interfaces;


namespace GPMS.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _studentRepository.GetAllAsync();
        }

        public async Task<Student?> GetByIdAsync(long id)
        {
            return await _studentRepository.GetByIdAsync(id);
        }

        public async Task<Student> CreateAsync(Student student)
        {
            return await _studentRepository.AddAsync(student);
        }

        public async Task<Student> UpdateAsync(Student student)
        {
            return await _studentRepository.UpdateAsync(student);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _studentRepository.DeleteAsync(id);
        }

        public Task<Student?> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<Student> AddAsync(Student entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }
    }
}
