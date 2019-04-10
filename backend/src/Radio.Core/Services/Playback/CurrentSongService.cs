using System.Threading.Tasks;
using Radio.Core.Domain.Playback;
using Radio.Core.Domain.Playback.Model;
using Radio.Core.Domain.Voting.Objects;

namespace Radio.Core.Services.Playback
{
    public class CurrentSongService : ICurrentSongService
    {
        private readonly ICurrentSongRepository _currentSongRepository;
        private readonly IClock _clock;

        public CurrentSongService(ICurrentSongRepository currentSongRepository, IClock clock)
        {
            _currentSongRepository = currentSongRepository;
            _clock = clock;
        }

        public async Task<CurrentSong> UpdateOrCreateAsync(SongWithVoteCount song)
        {
            var currentSong = await _currentSongRepository.GetOrDefaultAsync();
            if (currentSong == null)
            {
                currentSong = _currentSongRepository.Create();
                _currentSongRepository.Add(currentSong);
            }

            currentSong.Map(song, _clock);

            return currentSong;
        }
    }
}
