using System;
using System.ComponentModel.DataAnnotations;

namespace Radio.Core.Domain.MasterData.Model
{
    public class Image : EntityBase
    {
        [Required, StringLength(Constants.StringLengths.SHORT_NAME)]
        public string ContentType { get; set; }

        public int ContentLength { get; set; }

        public Guid FileId { get; set; }

        public virtual File File { get; set; }
    }
}
