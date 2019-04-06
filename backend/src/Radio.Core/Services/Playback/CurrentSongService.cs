using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Radio.Core.Domain.Playback;
using Radio.Core.Domain.Playback.Model;
using Radio.Core.Domain.Voting.Objects;

namespace Radio.Core.Services.Playback
{
    public class CurrentSongService : ICurrentSongService
    {
        private readonly ICurrentSongRepository _currentSongRepository;
        private readonly IClock _clock;
        private readonly ILogger _logger;

        public CurrentSongService(ICurrentSongRepository currentSongRepository, IClock clock, ILogger logger)
        {
            _currentSongRepository = currentSongRepository;
            _clock = clock;
            _logger = logger;
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

            _logger.LogInformation("Changing current song to {0}. Estimated end is {1}", currentSong.Song.FileName, currentSong.EndsAtTime.ToString());

            return currentSong;
        }
    }
}
