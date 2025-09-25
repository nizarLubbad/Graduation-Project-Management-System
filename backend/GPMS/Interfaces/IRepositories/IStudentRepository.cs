using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IStudentRepository : IBaseRepository<Student,long>
    {
        Task<string?> GetStudentNameAsync(long studentId);
        //Task<string?> GetByEmailAsync(string email);
        //Task SaveChangesAsync();
        Task<bool> ExistsByEmailAsync(string email);
        Task<Student?> GetByUserIdAsync(long userId);
        IQueryable<Student> GetAll();



    }
}