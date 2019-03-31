using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.Voting.Model;

namespace Radio.Core.Domain.Voting.Objects
{
    public class SongWithVoteCount
    {
        public Song Song { get; set; }

        public int VoteCount { get; set; }
    }
}
