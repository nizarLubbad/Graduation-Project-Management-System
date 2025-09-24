using GPMS.DTOS.Student;

namespace GPMS.Interfaces
{
    public interface IStudentService 
    {
        //    Task<string?> GetStudentNameAsync(long studentId);
        //    //Task<StudentDto?> GetByEmailAsync(string email);
        //Task<StudentProfileDto?> GetStudentProfileAsync(long userId);
        //Task<bool> UpdateStudentProfileAsync(long userId, UpdateStudentProfileDto dto);
        Task<List<StudentProfileDto>> GetAllStudentsAsync();
        Task UpdateStudentsStatusAsync(IEnumerable<long> studentIds);
    }
}
