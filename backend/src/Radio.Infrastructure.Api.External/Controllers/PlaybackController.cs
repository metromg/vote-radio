using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Radio.Core.Domain.Playback;
using Radio.Infrastructure.Api.External.Dtos;

namespace Radio.Infrastructure.Api.External.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PlaybackController : Controller
    {
        private readonly ICurrentSongRepository _currentSongRepository;
        private readonly IMapper _mapper;

        public PlaybackController(ICurrentSongRepository currentSongRepository, IMapper mapper)
        {
            _currentSongRepository = currentSongRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<CurrentSongDto> GetCurrentSongAsync()
        {
            var currentSong = await _currentSongRepository.GetOrDefaultAsync();

            return _mapper.Map<CurrentSongDto>(currentSong);
        }
    }
}
