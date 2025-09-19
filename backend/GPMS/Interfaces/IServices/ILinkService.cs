using GPMS.DTOS.Link;

namespace GPMS.Interfaces
{
    public interface ILinkService : IBaseService<LinkDto>
    {
        Task<IEnumerable<LinkDto>> GetByStudentIdAsync(long studentId);

    }
}
