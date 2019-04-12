using System;
using System.Linq;
using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using Radio.Core;
using Radio.Core.Domain.MasterData;
using Radio.Core.Services.MasterData;
using Radio.Infrastructure.Synchronization.Services;

namespace Radio.Tests.Integration.Infrastructure.Synchronization
{
    [TestFixture]
    public class SongImportServiceTests
    {
        private IClock _clock;
        private IOptions<EnvironmentOptions> _environmentOptions;

        private ISongImportService _songImportService;

        private IUnitOfWorkFactory<ISongRepository, IImageService> _songFactory;

        [SetUp]
        public void Setup()
        {
            var rootContainer = IntegrationTestHelper.SetUp().Build();

            _clock = Substitute.For<IClock>();
            _clock.UtcNow.Returns(new DateTime(2022, 2, 22, 12, 22, 22, DateTimeKind.Utc));

            _environmentOptions = Substitute.For<IOptions<EnvironmentOptions>>();

            _songImportService = new SongImportService(rootContainer.Resolve<ILogger>(), _clock, _environmentOptions, rootContainer.Resolve<IUnitOfWorkFactory<ISongRepository, IImageService>>());

            _songFactory = rootContainer.Resolve<IUnitOfWorkFactory<ISongRepository, IImageService>>();
        }

        [TearDown]
        public void Teardown()
        {
            using (var unit = _songFactory.Begin())
            {
                var songs = unit.Dependent.Get();
                foreach (var song in songs)
                {
                    song.RemoveCoverImage(unit.Dependent2);
                    unit.Dependent.Remove(song);
                }

                unit.Commit();
            }
        }

        [Test]
        public void Import_WithNoExistingSongs_ShouldAddNewSongs()
        {
            // Arrange
            _environmentOptions.Value.Returns(new EnvironmentOptions
            {
                DataDirectoryPath = "Infrastructure.Synchronization/TestData/1_data_add"
            });

            // Act
            _songImportService.Import();

            // Assert
            using (var unit = _songFactory.Begin())
            {
                var songs = unit.Dependent.Get().OrderBy(s => s.FileName).ToArray();

                Assert.That(songs.Length, Is.EqualTo(4));

                Assert.That(songs[0].Title, Is.EqualTo("Test Title"));
                Assert.That(songs[0].Album, Is.EqualTo("Test Album"));
                Assert.That(songs[0].Artist, Is.EqualTo("Test Artist"));
                Assert.That(songs[0].CoverImageId, Is.Not.Null);
                Assert.That(songs[0].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[0].FileName, Is.EqualTo("album1/1.mp3"));
                Assert.That(songs[0].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));

                Assert.That(songs[1].Title, Is.EqualTo("Test Title 2"));
                Assert.That(songs[1].Album, Is.EqualTo("Test Album 2"));
                Assert.That(songs[1].Artist, Is.EqualTo("Test Artist 2"));
                Assert.That(songs[1].CoverImageId, Is.Not.Null);
                Assert.That(songs[1].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[1].FileName, Is.EqualTo("album1/2.mp3"));
                Assert.That(songs[1].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));

                Assert.That(songs[2].Title, Is.EqualTo("Test Title 3"));
                Assert.That(songs[2].Album, Is.EqualTo("Test Album 3"));
                Assert.That(songs[2].Artist, Is.EqualTo("Test Artist 3"));
                Assert.That(songs[2].CoverImageId, Is.Not.Null);
                Assert.That(songs[2].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[2].FileName, Is.EqualTo("album2/1.mp3"));
                Assert.That(songs[2].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));

                Assert.That(songs[3].Title, Is.EqualTo("Test Title 2"));
                Assert.That(songs[3].Album, Is.EqualTo("Test Album 2"));
                Assert.That(songs[3].Artist, Is.EqualTo("Test Artist 2"));
                Assert.That(songs[3].CoverImageId, Is.Not.Null);
                Assert.That(songs[3].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[3].FileName, Is.EqualTo("album2/2.mp3"));
                Assert.That(songs[3].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));
            }
        }

        [Test]
        public void Import_WithUpdatedSongs_UpdatesExistingSongs()
        {
            // Arrange
            _environmentOptions.Value.Returns(new EnvironmentOptions
            {
                DataDirectoryPath = "Infrastructure.Synchronization/TestData/1_data_add"
            });

            _songImportService.Import();

            _environmentOptions.Value.Returns(new EnvironmentOptions
            {
                DataDirectoryPath = "Infrastructure.Synchronization/TestData/2_data_update"
            });

            _clock.UtcNow.Returns(new DateTime(2022, 2, 22, 12, 22, 23, DateTimeKind.Utc));

            // Act
            _songImportService.Import();

            // Assert
            using (var unit = _songFactory.Begin())
            {
                var songs = unit.Dependent.Get().OrderBy(s => s.FileName).ToArray();

                Assert.That(songs.Length, Is.EqualTo(4));

                Assert.That(songs[0].Title, Is.EqualTo("Test Title 3"));
                Assert.That(songs[0].Album, Is.EqualTo("Test Album 3"));
                Assert.That(songs[0].Artist, Is.EqualTo("Test Artist 3"));
                Assert.That(songs[0].CoverImageId, Is.Not.Null);
                Assert.That(songs[0].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[0].FileName, Is.EqualTo("album1/1.mp3"));
                Assert.That(songs[0].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));

                Assert.That(songs[1].Title, Is.EqualTo("Test Title 2"));
                Assert.That(songs[1].Album, Is.EqualTo("Test Album 2"));
                Assert.That(songs[1].Artist, Is.EqualTo("Test Artist 2"));
                Assert.That(songs[1].CoverImageId, Is.Not.Null);
                Assert.That(songs[1].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[1].FileName, Is.EqualTo("album1/2.mp3"));
                Assert.That(songs[1].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));

                Assert.That(songs[2].Title, Is.EqualTo("Test Title 3"));
                Assert.That(songs[2].Album, Is.EqualTo("Test Album 3"));
                Assert.That(songs[2].Artist, Is.EqualTo("Test Artist 3"));
                Assert.That(songs[2].CoverImageId, Is.Null);
                Assert.That(songs[2].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[2].FileName, Is.EqualTo("album2/1.mp3"));
                Assert.That(songs[2].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));

                Assert.That(songs[3].Title, Is.EqualTo("Test Title 2"));
                Assert.That(songs[3].Album, Is.EqualTo("Test Album 2"));
                Assert.That(songs[3].Artist, Is.EqualTo("Test Artist 2"));
                Assert.That(songs[3].CoverImageId, Is.Null);
                Assert.That(songs[3].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[3].FileName, Is.EqualTo("album2/2.mp3"));
                Assert.That(songs[3].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));
            }
        }

        [Test]
        public void Import_WithRemovedSongs_RemovesSongs()
        {
            // Arrange
            _environmentOptions.Value.Returns(new EnvironmentOptions
            {
                DataDirectoryPath = "Infrastructure.Synchronization/TestData/1_data_add"
            });

            _songImportService.Import();

            _environmentOptions.Value.Returns(new EnvironmentOptions
            {
                DataDirectoryPath = "Infrastructure.Synchronization/TestData/3_data_delete"
            });

            _clock.UtcNow.Returns(new DateTime(2022, 2, 22, 12, 22, 23, DateTimeKind.Utc));

            // Act
            _songImportService.Import();

            // Assert
            using (var unit = _songFactory.Begin())
            {
                var songs = unit.Dependent.Get().OrderBy(s => s.FileName).ToArray();

                Assert.That(songs.Length, Is.EqualTo(1));

                Assert.That(songs[0].Title, Is.EqualTo("Test Title"));
                Assert.That(songs[0].Album, Is.EqualTo("Test Album"));
                Assert.That(songs[0].Artist, Is.EqualTo("Test Artist"));
                Assert.That(songs[0].CoverImageId, Is.Null);
                Assert.That(songs[0].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[0].FileName, Is.EqualTo("album1/1.mp3"));
                Assert.That(songs[0].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));
            }
        }

        [Test]
        public void Import_WithInvalidSongs_DoesNotImportInvalidSongs()
        {
            // Arrange
            _environmentOptions.Value.Returns(new EnvironmentOptions
            {
                DataDirectoryPath = "Infrastructure.Synchronization/TestData/4_data_validate"
            });

            // Act
            _songImportService.Import();

            // Assert
            using (var unit = _songFactory.Begin())
            {
                var songs = unit.Dependent.Get().OrderBy(s => s.FileName).ToArray();

                Assert.That(songs.Length, Is.EqualTo(5));

                Assert.That(songs[0].Title, Is.EqualTo("Test Title"));
                Assert.That(songs[0].Album, Is.EqualTo("Test Album"));
                Assert.That(songs[0].Artist, Is.EqualTo("Test Artist"));
                Assert.That(songs[0].CoverImageId, Is.Null);
                Assert.That(songs[0].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[0].FileName, Is.EqualTo("album1/1.mp3"));
                Assert.That(songs[0].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));

                Assert.That(songs[1].Title, Is.EqualTo("Test Title"));
                Assert.That(songs[1].Album, Is.EqualTo("Test Album"));
                Assert.That(songs[1].Artist, Is.EqualTo("Test Artist"));
                Assert.That(songs[1].CoverImageId, Is.Null);
                Assert.That(songs[1].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[1].FileName, Is.EqualTo("album1/2.mp3"));
                Assert.That(songs[1].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));

                Assert.That(songs[2].Title, Is.EqualTo("Test Title"));
                Assert.That(songs[2].Album, Is.EqualTo("Test Album"));
                Assert.That(songs[2].Artist, Is.EqualTo("Test Artist"));
                Assert.That(songs[2].CoverImageId, Is.Null);
                Assert.That(songs[2].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[2].FileName, Is.EqualTo("album1/3.mp3"));
                Assert.That(songs[2].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));

                Assert.That(songs[3].Title, Is.EqualTo("Test Title"));
                Assert.That(songs[3].Album, Is.EqualTo("Test Album"));
                Assert.That(songs[3].Artist, Is.EqualTo("Test Artist"));
                Assert.That(songs[3].CoverImageId, Is.Null);
                Assert.That(songs[3].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[3].FileName, Is.EqualTo("album1/4.mp3"));
                Assert.That(songs[3].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));

                Assert.That(songs[4].Title, Is.EqualTo("Test Title"));
                Assert.That(songs[4].Album, Is.EqualTo("Test Album"));
                Assert.That(songs[4].Artist, Is.EqualTo("Test Artist"));
                Assert.That(songs[4].CoverImageId, Is.Null);
                Assert.That(songs[4].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[4].FileName, Is.EqualTo("album1/5.mp3"));
                Assert.That(songs[4].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));
            }
        }

        [Test]
        public void Import_WithInvalidUpdatedSongs_RemovesInvalidSongs()
        {
            // Arrange
            _environmentOptions.Value.Returns(new EnvironmentOptions
            {
                DataDirectoryPath = "Infrastructure.Synchronization/TestData/4_data_validate"
            });

            _songImportService.Import();

            _environmentOptions.Value.Returns(new EnvironmentOptions
            {
                DataDirectoryPath = "Infrastructure.Synchronization/TestData/5_data_validate_update"
            });

            _clock.UtcNow.Returns(new DateTime(2022, 2, 22, 12, 22, 23, DateTimeKind.Utc));

            // Act
            _songImportService.Import();

            // Assert
            using (var unit = _songFactory.Begin())
            {
                var songs = unit.Dependent.Get().OrderBy(s => s.FileName).ToArray();

                Assert.That(songs.Length, Is.EqualTo(1));

                Assert.That(songs[0].Title, Is.EqualTo("Test Title"));
                Assert.That(songs[0].Album, Is.EqualTo("Test Album"));
                Assert.That(songs[0].Artist, Is.EqualTo("Test Artist"));
                Assert.That(songs[0].CoverImageId, Is.Null);
                Assert.That(songs[0].DurationInSeconds, Is.EqualTo(45));
                Assert.That(songs[0].FileName, Is.EqualTo("album1/1.mp3"));
                Assert.That(songs[0].LastImportDate.UtcDateTime, Is.EqualTo(_clock.UtcNow));
            }
        }
    }
}
