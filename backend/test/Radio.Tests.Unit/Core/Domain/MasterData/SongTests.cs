using System;
using NSubstitute;
using NUnit.Framework;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.MasterData.Objects;
using Radio.Core.Services.MasterData;

namespace Radio.Tests.Unit.Core.Domain.MasterData
{
    [TestFixture]
    public class SongTests
    {
        private IImageService _imageService;

        [SetUp]
        public void Setup()
        {
            _imageService = Substitute.For<IImageService>();
        }

        [Test]
        public void AttachOrReplaceCoverImage_WithExistingCoverImage_RemovesExistingCoverImage()
        {
            // Arrange
            var image = new Image();
            var song = new Song
            {
                CoverImageId = image.Id,
                CoverImage = image
            };

            // Act
            song.AttachOrReplaceCoverImage(null, _imageService);

            // Assert
            Assert.That(song.CoverImageId, Is.Null);
            Assert.That(song.CoverImage, Is.Null);

            _imageService.Received(1).RemoveImage(image);
            _imageService.Received(0).AddImage(Arg.Any<FileInfo>());
        }

        [Test]
        public void AttachOrReplaceCoverImage_WithNoExistingCoverImage_DoesNotRemoveImage()
        {
            // Arrange
            var song = new Song();

            // Act
            song.AttachOrReplaceCoverImage(null, _imageService);

            // Assert
            _imageService.Received(0).RemoveImage(Arg.Any<Image>());
            _imageService.Received(0).AddImage(Arg.Any<FileInfo>());
        }

        [Test]
        public void AttachOrReplaceCoverImage_WithNewCoverImage_AndExistingCoverImage_ReplacesExistingCoverImage()
        {
            // Arrange
            var image = new Image();
            var song = new Song
            {
                CoverImageId = image.Id,
                CoverImage = image
            };

            var fileInfo = new FileInfo("contentType", new byte[] { 1, 2, 3, 4 });

            _imageService.AddImage(fileInfo).Returns(new Image());

            // Act
            song.AttachOrReplaceCoverImage(fileInfo, _imageService);

            // Assert
            Assert.That(song.CoverImageId, Is.Not.Null);
            Assert.That(song.CoverImage, Is.Not.Null);
            Assert.That(song.CoverImageId, Is.Not.EqualTo(image.Id));

            _imageService.Received(1).RemoveImage(image);
            _imageService.Received(1).AddImage(fileInfo);
        }

        [Test]
        public void AttachOrReplaceCoverImage_WithNewCoverImage_AndNoExistingCoverImage_AttachesNewCoverImage()
        {
            // Arrange
            var song = new Song();

            var fileInfo = new FileInfo("contentType", new byte[] { 1, 2, 3, 4 });

            _imageService.AddImage(fileInfo).Returns(new Image());

            // Act
            song.AttachOrReplaceCoverImage(fileInfo, _imageService);

            // Assert
            Assert.That(song.CoverImageId, Is.Not.Null);
            Assert.That(song.CoverImage, Is.Not.Null);

            _imageService.Received(0).RemoveImage(Arg.Any<Image>());
            _imageService.Received(1).AddImage(fileInfo);
        }

        [Test]
        public void RemoveCoverImage_WithExistingImage_RemovesExistingImage()
        {
            // Arrange
            var image = new Image();
            var song = new Song
            {
                CoverImageId = image.Id,
                CoverImage = image
            };

            // Act
            song.RemoveCoverImage(_imageService);

            // Assert
            Assert.That(song.CoverImageId, Is.Null);
            Assert.That(song.CoverImage, Is.Null);

            _imageService.Received(1).RemoveImage(image);
        }

        [Test]
        public void RemoveCoverImage_WithNoExistingImage_DoesNotRemoveImage()
        {
            // Arrange
            var song = new Song();

            // Act
            song.RemoveCoverImage(_imageService);

            // Assert
            _imageService.Received(0).RemoveImage(Arg.Any<Image>());
        }

        [Test]
        public void Map_MapsAllProperties()
        {
            // Arrange
            var song = new Song();

            var title = "Test Title";
            var album = "Test Album";
            var artist = "Test Artist";
            var coverImageFile = new FileInfo("contentType", new byte[] { 1, 2, 3, 4 });
            var duration = TimeSpan.FromSeconds(123);
            var fileName = "Test Filename.mp3";

            var image = new Image();
            _imageService.AddImage(coverImageFile).Returns(image);

            // Act
            song.Map(title, album, artist, coverImageFile, duration, fileName, _imageService);

            // Assert
            Assert.That(song.Title, Is.EqualTo(title));
            Assert.That(song.Album, Is.EqualTo(album));
            Assert.That(song.Artist, Is.EqualTo(artist));
            Assert.That(song.CoverImageId, Is.EqualTo(image.Id));
            Assert.That(song.CoverImage, Is.SameAs(image));
            Assert.That(song.DurationInSeconds, Is.EqualTo(duration.TotalSeconds));
            Assert.That(song.FileName, Is.EqualTo(fileName));
        }
    }
}
