using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.Voting;
using Radio.Core.Domain.Voting.Model;
using Radio.Core.Domain.Voting.Objects;
using Radio.Core.Services;
using Radio.Core.Services.Playback;
using Radio.Core.Services.Voting;

namespace Radio.Tests.Unit.Core.Services
{
    [TestFixture]
    public class VotingFinisherTests
    {
        private IVotingCandidateRepository _votingCandidateRepository;
        private ISongRepository _songRepository;
        private ICurrentSongService _currentSongService;
        private IVotingCandidateService _votingCandidateService;
        private ILogger _logger;

        private IVotingFinisher _votingFinisher;

        [SetUp]
        public void Setup()
        {
            _votingCandidateRepository = Substitute.For<IVotingCandidateRepository>();
            _songRepository = Substitute.For<ISongRepository>();
            _currentSongService = Substitute.For<ICurrentSongService>();
            _votingCandidateService = Substitute.For<IVotingCandidateService>();
            _logger = Substitute.For<ILogger>();

            _votingFinisher = new VotingFinisher(_votingCandidateRepository, _songRepository, _currentSongService, _votingCandidateService, _logger);
        }

        [Test]
        public async Task CollectResultAndLock_WithWinnerOfVoting_ReturnsWinnerOfVoting()
        {
            // Arrange
            var winnerOfVoting = new SongWithVoteCount
            {
                Song = new Song { Id = Guid.NewGuid() },
                VoteCount = 22
            };

            _votingCandidateRepository.GetWinnerOfVotingWithVoteCountOrDefaultAsync().Returns(Task.FromResult(winnerOfVoting));

            // Act
            var result = await _votingFinisher.CollectResultAndLockAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(winnerOfVoting));
            Assert.That(result.Song.Id, Is.EqualTo(winnerOfVoting.Song.Id));
            Assert.That(result.VoteCount, Is.EqualTo(winnerOfVoting.VoteCount));
        }

        [Test]
        public async Task CollectResultAndLock_WithNoWinnerOfVoting_ReturnsRandomSong()
        {
            // Arrange
            var randomSong = new Song { Id = Guid.NewGuid() };

            _votingCandidateRepository.GetWinnerOfVotingWithVoteCountOrDefaultAsync().Returns(Task.FromResult(default(SongWithVoteCount)));
            _songRepository.GetRandomAsync(take: 1).Returns(Task.FromResult(new[] { randomSong }));

            // Act
            var result = await _votingFinisher.CollectResultAndLockAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Song.Id, Is.EqualTo(randomSong.Id));
            Assert.That(result.VoteCount, Is.EqualTo(0));
        }

        [Test]
        public async Task CollectResultAndLock_LocksCurrentVotingCandidates()
        {
            // Arrange
            var winnerOfVoting = new SongWithVoteCount
            {
                Song = new Song()
            };

            var currentVotingCandidates = new[]
            {
                new VotingCandidate(),
                new VotingCandidate(),
                new VotingCandidate()
            };

            _votingCandidateRepository.GetWinnerOfVotingWithVoteCountOrDefaultAsync().Returns(winnerOfVoting);
            _votingCandidateRepository.Get().Returns(currentVotingCandidates);

            // Act
            await _votingFinisher.CollectResultAndLockAsync();

            // Assert
            Assert.That(currentVotingCandidates[0].IsActive, Is.False);
            Assert.That(currentVotingCandidates[1].IsActive, Is.False);
            Assert.That(currentVotingCandidates[2].IsActive, Is.False);
        }
    }
}
