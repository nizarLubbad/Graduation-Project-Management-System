using GPMS.DTOS.Link;
using GPMS.Interfaces;
using GPMS.Models;

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
                Description = dto.Description,
                TeamId = dto.TeamId
            };

            return await _linkRepository.AddAsync(link);
        }

        public async Task<IEnumerable<Link>> GetLinksByTeamIdAsync(long teamId)
        {
            var allLinks = await _linkRepository.GetAllAsync();
            return allLinks.Where(l => l.TeamId == teamId);
        }

        public async Task<Link?> UpdateLinkAsync(int id, LinkDto dto)
        {
            var existingLink = await _linkRepository.GetByIdAsync(id);
            if (existingLink == null) return null;

            existingLink.Url = dto.Url;
            existingLink.Title = dto.Title;
            existingLink.Description = dto.Description;
            existingLink.TeamId = dto.TeamId;

            return await _linkRepository.UpdateAsync(existingLink);
        }

        public async Task<bool> DeleteLinkAsync(int id)
        {
            return await _linkRepository.DeleteAsync(id);
        }
    }
}
