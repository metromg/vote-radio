using System;
using System.ComponentModel.DataAnnotations;

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

        public int DurationInSeconds { get; set; }

        [Required, StringLength(Constants.StringLengths.LONG_NAME)]
        public string FileName { get; set; }
    }
}
