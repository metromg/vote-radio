using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.MasterData.Objects;

namespace Radio.Core.Services.MasterData
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IFileRepository _fileRepository;

        public ImageService(IImageRepository imageRepository, IFileRepository fileRepository)
        {
            _imageRepository = imageRepository;
            _fileRepository = fileRepository;
        }

        public FileInfo FileFromImage(Image image)
        {
            return new FileInfo(image, () => 
            {
                var file = _fileRepository.GetById(image.FileId);
                return new System.IO.MemoryStream(file.Data);
            });
        }

        public Image AddImage(FileInfo fileInfo)
        {
            var image = _imageRepository.Create();
            image.AddFile(fileInfo, _fileRepository);

            _imageRepository.Add(image);

            return image;
        }

        public void RemoveImage(Image image)
        {
            _fileRepository.Remove(image.File);
            _imageRepository.Remove(image);
        }
    }
}
