using System.ComponentModel.DataAnnotations;

namespace Radio.Core.Domain
{
    public class Song : EntityBase
    {
        [Required, StringLength(Constants.StringLengths.NAME)]
        public string Title { get; set; }
    }
}
