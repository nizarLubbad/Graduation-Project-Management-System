using GPMS.Models;
using GPMS.Interfaces;
using GPMS.DTOS.Link;

namespace GPMS.Services
{
    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;

        public LinkService(ILinkRepository linkRepository)
        {
            _linkRepository = linkRepository;
        }

        public async Task<Link> AddLinkAsync(CreateLinkDto dto)
        {
            var link = new Link
            {
                Url = dto.Url,
                Title = dto.Title,
                TeamId = dto.TeamId,
                StudentId = dto.StudentId,
                Date = DateTime.Now
            };

            return await _linkRepository.AddAsync(link);
        }

        public async Task<IEnumerable<Link>> GetByTeamIdAsync(long teamId)
        {
            return await _linkRepository.GetByTeamIdAsync(teamId);
        }

        public async Task<Link?> GetByIdAsync(long id)
        {
            return await _linkRepository.GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _linkRepository.DeleteAsync(id);
        }
    }
}
