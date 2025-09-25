using System.Collections.Generic;
using System.Threading.Tasks;
namespace GPMS.Interfaces
{
    public interface IBaseRepository<T, TId> where T : class
    {
        Task  <IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(TId id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(TId id);
        Task<int> SaveChangesAsync();

    }
}
