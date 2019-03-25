using System;
using Radio.Core.Domain.MasterData;

namespace Radio.Core.Domain.Playback
{
    public class CurrentSong : EntityBase
    {
        public Guid SongId { get; set; }

        public virtual Song Song { get; set; }

        public int VoteCount { get; set; }

        public DateTime EndsAtTime { get; set; }
    }
}
