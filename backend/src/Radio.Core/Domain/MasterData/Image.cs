using System;
using System.ComponentModel.DataAnnotations;

namespace Radio.Core.Domain.MasterData
{
    public class Image : EntityBase
    {
        [Required, StringLength(Constants.StringLengths.SHORT_NAME)]
        public string ContentType { get; set; }

        public int ContentLength { get; set; }

        public Guid FileId { get; set; }

        public File File { get; set; }
    }
}
