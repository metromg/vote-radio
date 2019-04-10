using NUnit.Framework;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.MasterData.Objects;

namespace Radio.Tests.Unit.Core.Domain.MasterData
{
    [TestFixture]
    public class FileInfoTests
    {
        [Test]
        public void Constructor_WithByteArray_CreatesValidFileInfo()
        {
            // Arrange
            var contentType = "contentType";
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            // Act
            var fileInfo = new FileInfo(contentType, data);

            // Assert
            Assert.That(fileInfo.ContentType, Is.EqualTo("contentType"));
            Assert.That(fileInfo.ContentLength, Is.EqualTo(8));
            Assert.That(fileInfo.ToByteArray(), Is.EqualTo(data));
        }

        [Test]
        public void Constructor_WithStream_CreatesValidFileInfo()
        {
            // Arrange
            var contentType = "contentType";
            var contentLength = 8;
            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var stream = new System.IO.MemoryStream(data);

            // Act
            var fileInfo = new FileInfo(contentType, contentLength, stream);

            // Assert
            Assert.That(fileInfo.ContentType, Is.EqualTo("contentType"));
            Assert.That(fileInfo.ContentLength, Is.EqualTo(8));
            Assert.That(fileInfo.ToByteArray(), Is.EqualTo(data));

            stream.Dispose();
        }

        [Test]
        public void Constructor_WithImage_CreatesValidFileInfo()
        {
            // Arrange
            var image = new Image
            {
                ContentType = "contentType",
                ContentLength = 8
            };

            var data = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            // Act
            var fileInfo = new FileInfo(image, () => new System.IO.MemoryStream(data));

            // Assert
            Assert.That(fileInfo.ContentType, Is.EqualTo("contentType"));
            Assert.That(fileInfo.ContentLength, Is.EqualTo(8));
            Assert.That(fileInfo.ToByteArray(), Is.EqualTo(data));
        }
    }
}
