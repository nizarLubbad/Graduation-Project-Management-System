using GPMS.Interfaces;
using GPMS.Models;

namespace GPMS.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        //get students depending on their status (status =false)
        

        //basic crud operations
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _studentRepository.GetAllAsync();
        }

        public async Task<Student?> GetByIdAsync(object id)
        {
            if (id is long studentId)
            {
                return await _studentRepository.GetByIdAsync(studentId);
            }
            return null;
        }

        public async Task<Student> AddAsync(Student entity)
        {
            return await _studentRepository.AddAsync(entity);
        }

        public async Task<Student> UpdateAsync(Student entity)
        {
            return await _studentRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(object id)
        {
            if (id is long studentId)
            {
                return await _studentRepository.DeleteAsync(studentId);
            }
            return false;
        }

    }
}
