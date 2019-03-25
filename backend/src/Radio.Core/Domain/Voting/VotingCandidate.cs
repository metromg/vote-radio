using System;
using Radio.Core.Domain.MasterData;

namespace Radio.Core.Domain.Voting
{
    public class VotingCandidate : EntityBase
    {
        public Guid SongId { get; set; }

        public virtual Song Song { get; set; }

        public int DisplayOrder { get; set; }
    }
}
