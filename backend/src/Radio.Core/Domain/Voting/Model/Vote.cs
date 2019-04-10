using System;
using System.ComponentModel.DataAnnotations;

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

        public void Validate()
        {
            if (VotingCandidate.IsActive == false)
            {
                throw new ValidationException("Cannot create or update vote for inactive voting candidate.");
            }
        }
    }
}
