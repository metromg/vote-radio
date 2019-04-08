using NUnit.Framework;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.Voting.Model;

namespace Radio.Tests.Unit.Core.Domain.Voting
{
    [TestFixture]
    public class VotingCandidateTests
    {
        [Test]
        public void VotingCandidateIsActiveInitially()
        {
            // Arrange
            var votingCandidate = new VotingCandidate();

            // Assert
            Assert.That(votingCandidate.IsActive, Is.True);
        }

        [Test]
        public void Map_MapsAllProperties()
        {
            // Arrange
            var votingCandidate = new VotingCandidate();
            var song = new Song();
            var displayOrder = 22;

            // Act
            votingCandidate.Map(song, displayOrder);

            // Assert
            Assert.That(votingCandidate.SongId, Is.EqualTo(song.Id));
            Assert.That(votingCandidate.Song, Is.EqualTo(song));
            Assert.That(votingCandidate.DisplayOrder, Is.EqualTo(displayOrder));
            Assert.That(votingCandidate.Votes, Is.Not.Null);
        }
    }
}
