using System;

namespace Radio.Core.Domain.Voting.Model
{
    public class Vote : EntityBase
    {
        public Guid VotingCandidateId { get; set; }

        public virtual VotingCandidate VotingCandidate { get; set; }

        public Guid UserIdentifier { get; set; }

        public void Map(VotingCandidate votingCandidate, Guid userIdentifier)
        {
            VotingCandidateId = votingCandidate.Id;
            VotingCandidate = votingCandidate;
            UserIdentifier = userIdentifier;
        }
    }
}
