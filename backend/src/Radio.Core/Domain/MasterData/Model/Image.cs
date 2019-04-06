using System;
using System.ComponentModel.DataAnnotations;
using Radio.Core.Domain.MasterData.Objects;

namespace Radio.Core.Domain.MasterData.Model
{
    public class Image : EntityBase
    {
        [Required, StringLength(Constants.StringLengths.SHORT_NAME)]
        public string ContentType { get; set; }

        public long ContentLength { get; set; }

        public Guid FileId { get; set; }

        public virtual File File { get; set; }

        public void AddFile(FileInfo fileInfo, IFileRepository fileRepository)
        {
            ContentType = fileInfo.ContentType;
            ContentLength = fileInfo.ContentLength;

            var file = fileRepository.Create();
            file.Data = fileInfo.ToByteArray();
            fileRepository.Add(file);

            FileId = file.Id;
            File = file;
        }
    }
}
