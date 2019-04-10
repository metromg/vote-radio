using System;

namespace Radio.Infrastructure.Api.External.Dtos
{
    public class CurrentSongDto : EntityBaseDto
    {
        public Guid SongId { get; set; }

        public string Title { get; set; }

        public string Album { get; set; }

        public string Artist { get; set; }

        public Guid? CoverImageId { get; set; }

        public int VoteCount { get; set; }

        public int DurationInSeconds { get; set; }

        public DateTime EndsAtTime { get; set; }
    }
}
