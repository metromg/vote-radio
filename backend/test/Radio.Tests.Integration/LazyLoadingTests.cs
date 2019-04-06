using System;
using Autofac;
using NUnit.Framework;
using Radio.Core;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.Playback;

namespace Radio.Tests.Integration
{
    [TestFixture]
    public class LazyLoadingTests
    {
        private IUnitOfWorkFactory<ICurrentSongRepository, ISongRepository> _factory;

        private Guid _songId;

        [SetUp]
        public void Setup()
        {
            var rootContainer = IntegrationTestHelper.SetUp().Build();

            _factory = rootContainer.Resolve<IUnitOfWorkFactory<ICurrentSongRepository, ISongRepository>>();

            using (var unit = _factory.Begin())
            {
                var song = unit.Dependent2.Create();
                song.Title = "LazyLoadingTests";
                song.DurationInSeconds = 120;
                song.FileName = "LazyLoadingTests.mp3";

                unit.Dependent2.Add(song);
                unit.Commit();

                _songId = song.Id;
            }
        }

        [TearDown]
        public void Teardown()
        {
            using (var unit = _factory.Begin())
            {
                var song = unit.Dependent2.GetById(_songId);

                unit.Dependent2.Remove(song);
                unit.Commit();
            }
        }

        [Test]
        public void TestLazyLoadingAfterEntityCreation()
        {
            using (var unit = _factory.Begin())
            {
                // Arrange
                var currentSong = unit.Dependent.Create();
                currentSong.SongId = _songId;

                unit.Dependent.Add(currentSong);

                // Act
                var song = currentSong.Song;

                // Assert
                Assert.That(song, Is.Not.Null);
            }
        }
    }
}
