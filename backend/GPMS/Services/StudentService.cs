using GPMS.DTOS.Student;
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

        public async Task<IEnumerable<StudentDto>> GetAllAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Select(s => new StudentDto
            {
                Id = s.StudentId,
                Name = s.Name,
                Email = s.Email,
                Department = s.Department
            });
        }

        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return null;

            return new StudentDto
            {
                Id = student.StudentId,
                Name = student.Name,
                Email = student.Email,
                Department = student.Department
            };
        }
        public Task<StudentDto> CreateAsync(StudentDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }



        public Task<StudentDto?> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetStudentNameAsync(long studentId)
        {
            throw new NotImplementedException();
        }

        public Task<StudentDto?> UpdateAsync(object id, StudentDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
