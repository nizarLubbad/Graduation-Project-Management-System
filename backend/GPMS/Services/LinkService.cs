using GPMS.Models;
using GPMS.Interfaces;
using GPMS.DTOS.Link;
using AutoMapper;

namespace GPMS.Services
{
    //public class LinkService : ILinkService
    //{
    //    private readonly ILinkRepository _linkRepository;
    //    private readonly IMapper _mapper;

    //public LinkService(ILinkRepository linkRepository,IMapper mapper)
    //{
    //    _linkRepository = linkRepository;
    //    _mapper = mapper;
    //}

    //public async Task<LinkDto> CreateAsync(LinkDto dto)
    //{
    //    var link = new Link
    //    {
    //        Url = dto.Url,
    //        Title = dto.Title,
    //        TeamId = dto.TeamId,
    //        StudentId = dto.StudentId,
    //        Date = DateTime.Now
    //    };

    //    return await _linkRepository.AddAsync(link);
    //}

    //public async Task<IEnumerable<Link>> GetByTeamIdAsync(long teamId)
    //{
    //    return await _linkRepository.GetByTeamIdAsync(teamId);
    //}

    //public async Task<Link?> GetByIdAsync(long id)
    //{
    //    return await _linkRepository.GetByIdAsync(id);
    //}

    //public async Task<bool> DeleteAsync(long id)
    //{
    //    return await _linkRepository.DeleteAsync(id);
    //}

    public class LinkService : ILinkService
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IMapper _mapper;

        public LinkService(ILinkRepository linkRepository, IMapper mapper)
        {
            _linkRepository = linkRepository;
            _mapper = mapper;
        }

        // Create a new link
        public async Task<LinkDto> CreateAsync(LinkDto dto)
        {
            var link = new Link
            {
                Url = dto.Url,
                Title = dto.Title,
                TeamId = dto.TeamId,
                StudentId = dto.StudentId,
                Date = DateTime.Now
            };

            var saved = await _linkRepository.AddAsync(link);
            return _mapper.Map<LinkDto>(saved);
        }

        public async Task<LinkDto?> GetByIdAsync(object id)
        {
            var link = await _linkRepository.GetByIdAsync(id);
            if (link == null) return null;

            return _mapper.Map<LinkDto>(link);
        }

        public async Task<IEnumerable<LinkDto>> GetAllAsync()
        {
            var links = await _linkRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LinkDto>>(links);
        }

        public async Task<LinkDto?> UpdateAsync(object id, LinkDto dto)
        {
            var link = await _linkRepository.GetByIdAsync(id);
            if (link == null) return null;

            link.Url = dto.Url;
            link.Title = dto.Title;
            link.StudentId = dto.StudentId;
            link.TeamId = dto.TeamId;

            var updated = await _linkRepository.UpdateAsync(link);
            return _mapper.Map<LinkDto>(updated);
        }

        public async Task<bool> DeleteAsync(object id)
        {
            return await _linkRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<LinkDto>> GetByTeamIdAsync(long teamId)
        {
            var links = await _linkRepository.GetAllAsync();
            var filtered = links.Where(l => l.TeamId == teamId);
            return _mapper.Map<IEnumerable<LinkDto>>(filtered);
        }

        public async Task<IEnumerable<LinkDto>> GetByStudentIdAsync(long studentId)
        {
            var links = await _linkRepository.GetAllAsync();
            var filtered = links.Where(l => l.StudentId == studentId);
            return _mapper.Map<IEnumerable<LinkDto>>(filtered);
        }
    }


}
}
