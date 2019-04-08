using System;
using System.ComponentModel.DataAnnotations;
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

        [Test]
        public void Validate_WithActiveVotingCandidate_DoesNotThrowException()
        {
            // Arrange
            var vote = new Vote();
            var votingCandidate = new VotingCandidate();

            vote.VotingCandidate = votingCandidate;

            // Act
            vote.Validate();

            // Assert
            Assert.Pass();
        }

        [Test]
        public void Validate_WithInactiveVotingCandidate_ThrowsException()
        {
            // Arrange
            var vote = new Vote();
            var votingCandidate = new VotingCandidate
            {
                IsActive = false
            };

            vote.VotingCandidate = votingCandidate;

            // Act & Assert
            Assert.Throws<ValidationException>(() => vote.Validate());
        }
    }
}
