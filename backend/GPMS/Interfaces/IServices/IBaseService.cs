
namespace GPMS.Interfaces
{
    public interface IBaseService<TDto> where TDto : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(object id);
        Task<TDto> CreateAsync(TDto dto);
        Task<TDto?> UpdateAsync(object id, TDto dto);
        Task<bool> DeleteAsync(object id);
    }
}
