using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Radio.Core.Domain.Voting;
using Radio.Core.Domain.Voting.Model;
using Radio.Core.Services.Voting;

namespace Radio.Tests.Unit.Radio.Core.Services.Voting
{
    [TestFixture]
    public class VoteServiceTests
    {
        private IVoteRepository _voteRepository;

        private IVoteService _voteService;

        [SetUp]
        public void Setup()
        {
            _voteRepository = Substitute.For<IVoteRepository>();

            _voteService = new VoteService(_voteRepository);
        }

        [Test]
        public async Task UpdateOrCreate_WithNoUserVote_CreatesNewVote()
        {
            // Arrange
            var votingCandidate = new VotingCandidate();
            var userIdentifier = Guid.NewGuid();

            _voteRepository.GetByUserIdentifierOrDefaultAsync(userIdentifier).Returns(Task.FromResult(default(Vote)));
            _voteRepository.Create().Returns(new Vote());

            // Act
            var vote = await _voteService.UpdateOrCreateAsync(votingCandidate, userIdentifier);

            // Assert
            Assert.That(vote.VotingCandidateId, Is.EqualTo(votingCandidate.Id));
            Assert.That(vote.VotingCandidate, Is.EqualTo(votingCandidate));
            Assert.That(vote.UserIdentifier, Is.EqualTo(userIdentifier));

            _voteRepository.Received(1).Create();
            _voteRepository.Received(1).Add(vote);
        }

        [Test]
        public async Task UpdateOrCreate_WithUserVote_DoesNotCreateNewVote()
        {
            // Arrange
            var votingCandidate = new VotingCandidate();
            var userIdentifier = Guid.NewGuid();

            _voteRepository.GetByUserIdentifierOrDefaultAsync(userIdentifier).Returns(Task.FromResult(new Vote()));

            // Act
            var vote = await _voteService.UpdateOrCreateAsync(votingCandidate, userIdentifier);

            // Assert
            Assert.That(vote.VotingCandidateId, Is.EqualTo(votingCandidate.Id));
            Assert.That(vote.VotingCandidate, Is.EqualTo(votingCandidate));
            Assert.That(vote.UserIdentifier, Is.EqualTo(userIdentifier));

            _voteRepository.Received(0).Create();
            _voteRepository.Received(0).Add(vote);
        }
    }
}
