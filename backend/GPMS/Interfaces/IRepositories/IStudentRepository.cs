using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<string?> GetStudentNameAsync(long studentId);
        Task<string?> GetByEmailAsync(string email);
        Task SaveChangesAsync();

    }
}