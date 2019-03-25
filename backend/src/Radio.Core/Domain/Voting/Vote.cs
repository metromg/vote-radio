using System;

namespace Radio.Core.Domain.Voting
{
    public class Vote : EntityBase
    {
        public Guid VotingCandidateId { get; set; }

        public virtual VotingCandidate VotingCandidate { get; set; }
    }
}
