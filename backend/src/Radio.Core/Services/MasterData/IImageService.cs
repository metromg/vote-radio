using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.MasterData.Objects;

namespace Radio.Core.Services.MasterData
{
    public interface IImageService
    {
        FileInfo FileFromImage(Image image);

        Image AddImage(FileInfo fileInfo);

        void RemoveImage(Image image);
    }
}
