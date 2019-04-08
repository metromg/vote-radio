using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Radio.Core;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.Voting;

namespace Radio.Tests.Integration.Infrastructure.DbAccess.Voting
{
    [TestFixture]
    public class VotingCandidateRepositoryTests
    {
        private IUnitOfWorkFactory<IVotingCandidateRepository, IVoteRepository, ISongRepository> _factory;

        [SetUp]
        public void Setup()
        {
            var rootContainer = IntegrationTestHelper.SetUp().Build();

            _factory = rootContainer.Resolve<IUnitOfWorkFactory<IVotingCandidateRepository, IVoteRepository, ISongRepository>>();
        }

        [TearDown]
        public void Teardown()
        {
            using (var unit = _factory.Begin())
            {
                var votingCandidates = unit.Dependent.Get();
                foreach (var votingCandidate in votingCandidates)
                {
                    unit.Dependent.Remove(votingCandidate);
                }

                var songs = unit.Dependent3.Get();
                foreach (var song in songs)
                {
                    unit.Dependent3.Remove(song);
                }

                unit.Commit();
            }
        }

        [Test]
        public async Task GetWinnerOfVotingOrDefault_ReturnsCandidateWithHighestNumberOfVotes()
        {
            using (var unit = _factory.Begin())
            {
                // Arrange
                CreateAndAddVotingCandidate(1, 8, unit.Dependent, unit.Dependent2, unit.Dependent3);
                CreateAndAddVotingCandidate(2, 9, unit.Dependent, unit.Dependent2, unit.Dependent3);
                CreateAndAddVotingCandidate(3, 10, unit.Dependent, unit.Dependent2, unit.Dependent3);

                unit.Commit();
            }

            using (var unit = _factory.Begin())
            {
                // Act
                var result = await unit.Dependent.GetWinnerOfVotingWithVoteCountOrDefaultAsync();

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Song.Title, Is.EqualTo("SongRepositoryTests3"));
                Assert.That(result.Song.DurationInSeconds, Is.EqualTo(120));
                Assert.That(result.Song.FileName, Is.EqualTo("SongRepositoryTests.mp3"));
                Assert.That(result.VoteCount, Is.EqualTo(10));
            }
        }

        [Test]
        public async Task GetWinnerOfVotingOrDefault_WithEqualVotes_ReturnsRandomCandidate()
        {
            using (var unit = _factory.Begin())
            {
                // Arrange
                CreateAndAddVotingCandidate(1, 8, unit.Dependent, unit.Dependent2, unit.Dependent3);
                CreateAndAddVotingCandidate(2, 8, unit.Dependent, unit.Dependent2, unit.Dependent3);
                CreateAndAddVotingCandidate(3, 8, unit.Dependent, unit.Dependent2, unit.Dependent3);

                unit.Commit();
            }

            using (var unit = _factory.Begin())
            {
                // Act
                var result = await unit.Dependent.GetWinnerOfVotingWithVoteCountOrDefaultAsync();

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result.VoteCount, Is.EqualTo(8));
            }
        }

        [Test]
        public async Task GetWinnerOfVotingOrDefault_WithNoVotes_ReturnsRandomCandidate()
        {
            using (var unit = _factory.Begin())
            {
                // Arrange
                CreateAndAddVotingCandidate(1, 0, unit.Dependent, unit.Dependent2, unit.Dependent3);
                CreateAndAddVotingCandidate(2, 0, unit.Dependent, unit.Dependent2, unit.Dependent3);
                CreateAndAddVotingCandidate(3, 0, unit.Dependent, unit.Dependent2, unit.Dependent3);

                unit.Commit();
            }

            using (var unit = _factory.Begin())
            {
                // Act
                var result = await unit.Dependent.GetWinnerOfVotingWithVoteCountOrDefaultAsync();

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result.VoteCount, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task GetWinnerOfVotingOrDefault_WithNoVotingCandidates_ReturnsDefault()
        {
            using (var unit = _factory.Begin())
            {
                // Act
                var result = await unit.Dependent.GetWinnerOfVotingWithVoteCountOrDefaultAsync();

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        private static void CreateAndAddVotingCandidate(int number, int numberOfVotes, IVotingCandidateRepository votingCandidateRepository, IVoteRepository voteRepository, ISongRepository songRepository)
        {
            var song = songRepository.Create();
            song.Title = "SongRepositoryTests" + number;
            song.DurationInSeconds = 120;
            song.FileName = "SongRepositoryTests.mp3";

            songRepository.Add(song);

            var votingCandidate = votingCandidateRepository.Create();
            votingCandidate.SongId = song.Id;
            votingCandidate.Song = song;
            votingCandidate.DisplayOrder = number;

            votingCandidateRepository.Add(votingCandidate);

            foreach (var index in Enumerable.Range(0, numberOfVotes))
            {
                var vote = voteRepository.Create();
                vote.VotingCandidateId = votingCandidate.Id;
                vote.VotingCandidate = votingCandidate;
                vote.UserIdentifier = Guid.NewGuid();

                voteRepository.Add(vote);
            }
        }
    }
}
