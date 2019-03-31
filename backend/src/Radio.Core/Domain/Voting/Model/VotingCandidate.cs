using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Radio.Core.Domain.MasterData.Model;

namespace Radio.Core.Domain.Voting.Model
{
    public class VotingCandidate : EntityBase
    {
        public VotingCandidate()
        {
            Votes = new Collection<Vote>();
        }

        public Guid SongId { get; set; }

        public virtual Song Song { get; set; }

        public int DisplayOrder { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
