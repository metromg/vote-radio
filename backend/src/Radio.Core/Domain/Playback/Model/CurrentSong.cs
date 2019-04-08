using System;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.Voting.Objects;

namespace Radio.Core.Domain.Playback.Model
{
    public class CurrentSong : EntityBase
    {
        public Guid SongId { get; set; }

        public virtual Song Song { get; set; }

        public int VoteCount { get; set; }

        public DateTimeOffset EndsAtTime { get; set; }

        public void Map(SongWithVoteCount songWithVoteCount, IClock clock)
        {
            SongId = songWithVoteCount.Song.Id;
            Song = songWithVoteCount.Song;
            VoteCount = songWithVoteCount.VoteCount;
            EndsAtTime = new DateTimeOffset(clock.UtcNow)
                .AddSeconds(songWithVoteCount.Song.DurationInSeconds - Constants.App.CROSSFADE_DURATION_IN_SECONDS);
        }
    }
}
