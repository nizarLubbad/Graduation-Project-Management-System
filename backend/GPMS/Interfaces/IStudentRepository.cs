using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<string?> GetStudentNameAsync(long studentId);

    }
}