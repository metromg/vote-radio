using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Radio.Core;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.Playback;
using Radio.Core.Domain.Playback.Model;
using Radio.Core.Domain.Voting.Objects;
using Radio.Core.Services.Playback;

namespace Radio.Tests.Unit.Radio.Core.Services.Playback
{
    [TestFixture]
    public class CurrentSongServiceTests
    {
        private ICurrentSongRepository _currentSongRepository;
        private IClock _clock;
        private ILogger _logger;

        private ICurrentSongService _currentSongService;

        [SetUp]
        public void Setup()
        {
            _currentSongRepository = Substitute.For<ICurrentSongRepository>();
            _clock = Substitute.For<IClock>();
            _logger = Substitute.For<ILogger>();

            _clock.UtcNow.Returns(new DateTime(2022, 2, 22, 2, 22, 0, DateTimeKind.Utc));

            _currentSongService = new CurrentSongService(_currentSongRepository, _clock, _logger);
        }

        [Test]
        public async Task UpdateOrCreate_WithNoCurrentSong_CreatesNewCurrentSong()
        {
            // Arrange
            var songWithVoteCount = new SongWithVoteCount
            {
                Song = new Song { DurationInSeconds = 120 },
                VoteCount = 22
            };

            _currentSongRepository.GetOrDefaultAsync().Returns(Task.FromResult(default(CurrentSong)));
            _currentSongRepository.Create().Returns(new CurrentSong());

            // Act
            var currentSong = await _currentSongService.UpdateOrCreateAsync(songWithVoteCount);

            // Assert
            Assert.That(currentSong.SongId, Is.EqualTo(songWithVoteCount.Song.Id));
            Assert.That(currentSong.Song, Is.EqualTo(songWithVoteCount.Song));
            Assert.That(currentSong.VoteCount, Is.EqualTo(songWithVoteCount.VoteCount));
            Assert.That(currentSong.EndsAtTime.UtcDateTime, Is.EqualTo(_clock.UtcNow.AddSeconds(130)));

            _currentSongRepository.Received(1).Create();
            _currentSongRepository.Received(1).Add(currentSong);
        }

        [Test]
        public async Task UpdateOrCreate_WithCurrentSong_DoesNotCreateNewCurrentSong()
        {
            // Arrange
            var songWithVoteCount = new SongWithVoteCount
            {
                Song = new Song { DurationInSeconds = 120 },
                VoteCount = 22
            };

            _currentSongRepository.GetOrDefaultAsync().Returns(Task.FromResult(new CurrentSong()));

            // Act
            var currentSong = await _currentSongService.UpdateOrCreateAsync(songWithVoteCount);

            // Assert
            Assert.That(currentSong.SongId, Is.EqualTo(songWithVoteCount.Song.Id));
            Assert.That(currentSong.Song, Is.EqualTo(songWithVoteCount.Song));
            Assert.That(currentSong.VoteCount, Is.EqualTo(songWithVoteCount.VoteCount));
            Assert.That(currentSong.EndsAtTime.UtcDateTime, Is.EqualTo(_clock.UtcNow.AddSeconds(130)));

            _currentSongRepository.Received(0).Create();
            _currentSongRepository.Received(0).Add(currentSong);
        }
    }
}
