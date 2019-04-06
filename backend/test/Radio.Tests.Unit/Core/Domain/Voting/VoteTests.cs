using System;
using NUnit.Framework;
using Radio.Core.Domain.Voting.Model;

namespace Radio.Tests.Unit.Core.Domain.Voting
{
    [TestFixture]
    public class VoteTests
    {
        [Test]
        public void Map_MapsAllProperties()
        {
            // Arrange
            var vote = new Vote();
            var votingCandidate = new VotingCandidate();
            var userIdentifier = Guid.NewGuid();

            // Act
            vote.Map(votingCandidate, userIdentifier);

            // Assert
            Assert.That(vote.VotingCandidateId, Is.EqualTo(votingCandidate.Id));
            Assert.That(vote.VotingCandidate, Is.EqualTo(votingCandidate));
            Assert.That(vote.UserIdentifier, Is.EqualTo(userIdentifier));
        }
    }
}
