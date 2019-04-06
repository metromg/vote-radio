using NSubstitute;
using NUnit.Framework;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.MasterData.Objects;
using Radio.Core.Services.MasterData;

namespace Radio.Tests.Unit.Core.Services.MasterData
{
    [TestFixture]
    public class ImageServiceTests
    {
        private IImageRepository _imageRepository;
        private IFileRepository _fileRepository;

        private IImageService _imageService;

        [SetUp]
        public void Setup()
        {
            _imageRepository = Substitute.For<IImageRepository>();
            _fileRepository = Substitute.For<IFileRepository>();

            _imageService = new ImageService(_imageRepository, _fileRepository);
        }

        [Test]
        public void FileFromImage_ReturnsValidFileInfo()
        {
            // Arrange
            var file = new File
            {
                Data = new byte[] { 1, 2, 3, 4 }
            };

            var image = new Image
            {
                ContentType = "contentType",
                ContentLength = 8,
                FileId = file.Id
            };

            _fileRepository.GetById(file.Id).Returns(file);

            // Act
            var fileInfo = _imageService.FileFromImage(image);

            // Assert
            Assert.That(fileInfo.ContentType, Is.EqualTo(image.ContentType));
            Assert.That(fileInfo.ContentLength, Is.EqualTo(image.ContentLength));
            Assert.That(fileInfo.ToByteArray(), Is.EqualTo(file.Data));
            Assert.That(image.File, Is.Null);
        }

        [Test]
        public void AddImage_AddsANewImage()
        {
            // Arrange
            var fileInfo = new FileInfo("contentType", new byte[] { 1, 2, 3, 4 });

            _imageRepository.Create().Returns(new Image());
            _fileRepository.Create().Returns(new File());

            // Act
            var image = _imageService.AddImage(fileInfo);

            // Assert
            Assert.That(image.ContentType, Is.EqualTo(fileInfo.ContentType));
            Assert.That(image.ContentLength, Is.EqualTo(4));
            Assert.That(image.File, Is.Not.Null);
            Assert.That(image.File.Data, Is.EqualTo(fileInfo.ToByteArray()));
            Assert.That(image.FileId, Is.EqualTo(image.File.Id));

            _imageRepository.Received(1).Create();
            _imageRepository.Received(1).Add(image);
            _fileRepository.Received(1).Create();
            _fileRepository.Received(1).Add(image.File);
        }

        [Test]
        public void RemoveImage_RemovesImageAndFile()
        {
            // Arrange
            var image = new Image
            {
                File = new File()
            };

            // Act
            _imageService.RemoveImage(image);

            // Assert
            _imageRepository.Received(1).Remove(image);
            _fileRepository.Received(1).Remove(image.File);
        }
    }
}
