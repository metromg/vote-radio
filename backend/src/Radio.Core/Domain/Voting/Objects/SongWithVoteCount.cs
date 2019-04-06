using Radio.Core.Domain.MasterData.Model;

namespace Radio.Core.Domain.Voting.Objects
{
    public class SongWithVoteCount
    {
        public Song Song { get; set; }

        public int VoteCount { get; set; }
    }
}
