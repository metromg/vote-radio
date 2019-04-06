using System;
using NSubstitute;
using NUnit.Framework;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.MasterData.Objects;

namespace Radio.Tests.Unit.Core.Domain.MasterData
{
    [TestFixture]
    public class ImageTests
    {
        private IFileRepository _fileRepository;
        private Image _image;

        [SetUp]
        public void Setup()
        {
            _fileRepository = Substitute.For<IFileRepository>();
            _fileRepository.Create().Returns(new File());
            _image = new Image();
        }

        [Test]
        public void AddFile_AddsANewFileToTheImage()
        {
            // Arrange
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var fileInfo = new FileInfo(contentType: "contentType", data: data);

            // Act
            _image.AddFile(fileInfo, _fileRepository);

            // Assert
            Assert.That(_image.ContentType, Is.EqualTo("contentType"));
            Assert.That(_image.ContentLength, Is.EqualTo(8));
            Assert.That(_image.FileId, Is.Not.Null);
            Assert.That(_image.File, Is.Not.Null);
            Assert.That(_image.File.Data, Is.EqualTo(data));

            _fileRepository.Received(1).Create();
            _fileRepository.Received(1).Add(_image.File);
        }

        [Test]
        public void AddFile_WithEmptyFile_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _image.AddFile(null, _fileRepository));
        }
    }
}
