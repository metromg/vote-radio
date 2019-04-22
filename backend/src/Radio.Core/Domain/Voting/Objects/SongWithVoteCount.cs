using Radio.Core.Domain.MasterData.Model;

namespace Radio.Core.Domain.Voting.Objects
{
    public class SongWithVoteCount
    {
        public Song Song { get; set; }

        public int VoteCount { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
