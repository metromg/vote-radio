using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.Voting;
using Radio.Core.Domain.Voting.Model;
using Radio.Core.Services.Voting;

namespace Radio.Tests.Unit.Radio.Core.Services.Voting
{
    [TestFixture]
    public class VotingCandidateServiceTests
    {
        private IVotingCandidateRepository _votingCandidateRepository;

        private IVotingCandidateService _votingCandidateService;

        [SetUp]
        public void Setup()
        {
            _votingCandidateRepository = Substitute.For<IVotingCandidateRepository>();

            _votingCandidateService = new VotingCandidateService(_votingCandidateRepository);
        }

        [Test]
        public async Task UpdateOrCreate_RemovesAndAddsVotingCandidates()
        {
            // Arrange
            var existingCandidates = new[]
            {
                new VotingCandidate(),
                new VotingCandidate()
            };

            var newCandidateSongs = new[]
            {
                new Song(),
                new Song(),
                new Song()
            };

            _votingCandidateRepository.Get().Returns(existingCandidates);
            _votingCandidateRepository.Create().Returns(_ => new VotingCandidate());

            // Act
            var votingCandidates = await _votingCandidateService.UpdateOrCreateAsync(newCandidateSongs);

            // Assert
            Assert.That(votingCandidates.Length, Is.EqualTo(newCandidateSongs.Length));

            Assert.That(votingCandidates[0].SongId, Is.EqualTo(newCandidateSongs[0].Id));
            Assert.That(votingCandidates[0].Song, Is.EqualTo(newCandidateSongs[0]));
            Assert.That(votingCandidates[0].DisplayOrder, Is.EqualTo(0));
            Assert.That(votingCandidates[0].Votes, Is.Not.Null);

            Assert.That(votingCandidates[1].SongId, Is.EqualTo(newCandidateSongs[1].Id));
            Assert.That(votingCandidates[1].Song, Is.EqualTo(newCandidateSongs[1]));
            Assert.That(votingCandidates[1].DisplayOrder, Is.EqualTo(1));
            Assert.That(votingCandidates[1].Votes, Is.Not.Null);

            Assert.That(votingCandidates[2].SongId, Is.EqualTo(newCandidateSongs[2].Id));
            Assert.That(votingCandidates[2].Song, Is.EqualTo(newCandidateSongs[2]));
            Assert.That(votingCandidates[2].DisplayOrder, Is.EqualTo(2));
            Assert.That(votingCandidates[2].Votes, Is.Not.Null);

            _votingCandidateRepository.Received(1).Remove(existingCandidates[0]);
            _votingCandidateRepository.Received(1).Remove(existingCandidates[1]);

            _votingCandidateRepository.Received(3).Create();
            _votingCandidateRepository.Received(1).Add(votingCandidates[0]);
            _votingCandidateRepository.Received(1).Add(votingCandidates[1]);
            _votingCandidateRepository.Received(1).Add(votingCandidates[2]);
        }
    }
}
