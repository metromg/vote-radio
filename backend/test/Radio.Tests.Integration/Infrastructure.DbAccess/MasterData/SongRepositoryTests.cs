using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using NSubstitute;
using NUnit.Framework;
using Radio.Core;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.MasterData.Model;

namespace Radio.Tests.Integration.Infrastructure.DbAccess.MasterData
{
    [TestFixture]
    public class SongRepositoryTests
    {
        private IUnitOfWorkFactory<ISongRepository> _factory;
        private IClock _clock;

        [SetUp]
        public void Setup()
        {
            var rootContainer = IntegrationTestHelper.SetUp().Build();

            _factory = rootContainer.Resolve<IUnitOfWorkFactory<ISongRepository>>();
            _clock = Substitute.For<IClock>();
            _clock.UtcNow.Returns(new DateTime(2022, 2, 22, 12, 22, 22, DateTimeKind.Utc));
        }

        [TearDown]
        public void Teardown()
        {
            using (var unit = _factory.Begin())
            {
                var songs = unit.Dependent.Get();
                foreach (var song in songs)
                {
                    unit.Dependent.Remove(song);
                }

                unit.Commit();
            }
        }

        [Test]
        public async Task GetRandom_ReturnsExpectedNumberOfSongs()
        {
            // Arrange
            using (var unit = _factory.Begin())
            {
                CreateAndAddSong(1, unit.Dependent);
                CreateAndAddSong(2, unit.Dependent);
                CreateAndAddSong(3, unit.Dependent);
                CreateAndAddSong(4, unit.Dependent);
                CreateAndAddSong(5, unit.Dependent);
                CreateAndAddSong(6, unit.Dependent);

                unit.Commit();
            }

            using (var unit = _factory.Begin())
            {
                // Act
                var songs = await unit.Dependent.GetRandomAsync(take: 3);

                // Arrange
                Assert.That(songs.Length, Is.EqualTo(3));
            }
        }

        [Test]
        public void GetNextSongsToRemove_ReturnsExpectedSongs()
        {
            // Arrange
            using (var unit = _factory.Begin())
            {
                CreateAndAddSong(1, _clock.UtcNow.AddMilliseconds(-1), unit.Dependent);
                CreateAndAddSong(2, _clock.UtcNow, unit.Dependent);
                CreateAndAddSong(3, _clock.UtcNow.AddMilliseconds(1), unit.Dependent);

                unit.Commit();
            }

            using (var unit = _factory.Begin())
            {
                // Act
                var songs = unit.Dependent.GetNextSongsToRemove(_clock.UtcNow, batchSize: 3).ToArray();

                // Assert
                Assert.That(songs.Length, Is.EqualTo(1));
                Assert.That(songs[0].Title, Is.EqualTo("SongRepositoryTests1"));
                Assert.That(songs[0].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow.AddMilliseconds(-1)));
            }
        }

        [Test]
        public void GetNextSongsToRemove_ReturnsExpectedNumberOfSongs()
        {
            // Arrange
            using (var unit = _factory.Begin())
            {
                CreateAndAddSong(1, _clock.UtcNow.AddMilliseconds(-1), unit.Dependent);
                CreateAndAddSong(2, _clock.UtcNow.AddMilliseconds(-1), unit.Dependent);
                CreateAndAddSong(3, _clock.UtcNow.AddMilliseconds(-1), unit.Dependent);
                CreateAndAddSong(4, _clock.UtcNow.AddMilliseconds(-1), unit.Dependent);
                CreateAndAddSong(5, _clock.UtcNow.AddMilliseconds(-1), unit.Dependent);
                CreateAndAddSong(6, _clock.UtcNow.AddMilliseconds(-1), unit.Dependent);

                unit.Commit();
            }

            using (var unit = _factory.Begin())
            {
                // Act
                var songs = unit.Dependent.GetNextSongsToRemove(_clock.UtcNow, batchSize: 3).ToArray();

                // Assert
                Assert.That(songs.Length, Is.EqualTo(3));
            }
        }

        private static Song CreateAndAddSong(int number, ISongRepository songRepository)
        {
            var song = songRepository.Create();
            song.Title = "SongRepositoryTests" + number;
            song.DurationInSeconds = 120;
            song.FileName = "SongRepositoryTests.mp3";

            songRepository.Add(song);

            return song;
        }

        private static Song CreateAndAddSong(int number, DateTimeOffset lastImportDate, ISongRepository songRepository)
        {
            var song = CreateAndAddSong(number, songRepository);
            song.LastImportDate = lastImportDate;

            return song;
        }
    }
}
