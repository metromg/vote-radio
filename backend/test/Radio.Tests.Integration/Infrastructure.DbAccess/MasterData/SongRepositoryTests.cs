using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Radio.Core;
using Radio.Core.Domain.MasterData;

namespace Radio.Tests.Integration.Infrastructure.DbAccess.MasterData
{
    [TestFixture]
    public class SongRepositoryTests
    {
        private IUnitOfWorkFactory<ISongRepository> _factory;

        [SetUp]
        public void Setup()
        {
            var rootContainer = IntegrationTestHelper.SetUp().Build();

            _factory = rootContainer.Resolve<IUnitOfWorkFactory<ISongRepository>>();

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
            using (var unit = _factory.Begin())
            {
                // Arrange
                var take = 3;

                // Act
                var songs = await unit.Dependent.GetRandomAsync(take: take);

                // Arrange
                Assert.That(songs.Length, Is.EqualTo(take));
            }
        }

        private static void CreateAndAddSong(int number, ISongRepository songRepository)
        {
            var song = songRepository.Create();
            song.Title = "SongRepositoryTests" + number;
            song.DurationInSeconds = 120;
            song.FileName = "SongRepositoryTests.mp3";

            songRepository.Add(song);
        }
    }
}
