using GPMS.Models;

namespace GPMS.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, long>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        // GetStudentsByIdsAsync
        Task<IEnumerable<Student>> GetStudentsByIdsAsync(List<long> studentIds);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetByIdWithDetailsAsync(long userId);
        Task<int> DeleteAllUsersAsync();



    }
}
