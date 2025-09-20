using GPMS.DTOS.Student;

namespace GPMS.Interfaces
{
    public interface IStudentService : IBaseService<StudentDto>
    {
        Task<string?> GetStudentNameAsync(long studentId);
        //Task<StudentDto?> GetByEmailAsync(string email);
    }
}
