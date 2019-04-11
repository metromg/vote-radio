using System;
using System.ComponentModel.DataAnnotations;
using Radio.Core.Domain.MasterData.Objects;
using Radio.Core.Services.MasterData;

namespace Radio.Core.Domain.MasterData.Model
{
    public class Song : EntityBase
    {
        [Required, StringLength(Constants.StringLengths.NAME)]
        public string Title { get; set; }

        [StringLength(Constants.StringLengths.NAME)]
        public string Album { get; set; }

        [StringLength(Constants.StringLengths.NAME)]
        public string Artist { get; set; }

        public Guid? CoverImageId { get; set; }

        public virtual Image CoverImage { get; set; }

        public void AttachOrReplaceCoverImage(FileInfo file, IImageService imageService)
        {
            RemoveCoverImage(imageService);

            if (file != null)
            {
                CoverImage = imageService.AddImage(file);
                CoverImageId = CoverImage.Id;
            }
        }

        public void RemoveCoverImage(IImageService imageService)
        {
            if (CoverImageId.HasValue)
            {
                imageService.RemoveImage(CoverImage);

                CoverImageId = null;
                CoverImage = null;
            }
        }

        public int DurationInSeconds { get; set; }

        [Required, StringLength(Constants.StringLengths.LONG_NAME)]
        public string FileName { get; set; }

        public DateTimeOffset LastImportDate { get; set; }

        public void Map(string title, string album, string artist, FileInfo coverImageFile, TimeSpan duration, string fileName, IImageService imageService)
        {
            Title = title;
            Album = album;
            Artist = artist;

            AttachOrReplaceCoverImage(coverImageFile, imageService);

            DurationInSeconds = (int)duration.TotalSeconds;
            FileName = fileName;
        }
    }
}
