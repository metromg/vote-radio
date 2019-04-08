using System;
using NSubstitute;
using NUnit.Framework;
using Radio.Core;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.Playback.Model;
using Radio.Core.Domain.Voting.Objects;

namespace Radio.Tests.Unit.Core.Domain.Playback
{
    [TestFixture]
    public class CurrentSongTests
    {
        private IClock _clock;

        [SetUp]
        public void Setup()
        {
            _clock = Substitute.For<IClock>();
            _clock.UtcNow.Returns(new DateTime(2022, 2, 22, 2, 22, 0, DateTimeKind.Utc));
        }

        [Test]
        public void Map_MapsAllProperties()
        {
            // Arrange
            var currentSong = new CurrentSong();
            var songWithVoteCount = new SongWithVoteCount
            {
                Song = new Song { DurationInSeconds = 120 },
                VoteCount = 22
            };

            // Act
            currentSong.Map(songWithVoteCount, _clock);

            // Assert
            Assert.That(currentSong.SongId, Is.EqualTo(songWithVoteCount.Song.Id));
            Assert.That(currentSong.Song, Is.EqualTo(songWithVoteCount.Song));
            Assert.That(currentSong.VoteCount, Is.EqualTo(songWithVoteCount.VoteCount));
            Assert.That(currentSong.EndsAtTime.UtcDateTime, Is.EqualTo(_clock.UtcNow.AddSeconds(115)));
        }
    }
}
