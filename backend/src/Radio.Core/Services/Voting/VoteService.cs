using System;
using System.Threading.Tasks;
using Radio.Core.Domain.Voting;
using Radio.Core.Domain.Voting.Model;

namespace Radio.Core.Services.Voting
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;

        public VoteService(IVoteRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public async Task UpdateOrCreateAsync(VotingCandidate votingCandidate, Guid userIdentifier)
        {
            var vote = await _voteRepository.GetByUserIdentifierOrDefaultAsync(userIdentifier);
            if (vote == null)
            {
                vote = _voteRepository.Create();
                _voteRepository.Add(vote);
            }

            vote.Map(votingCandidate, userIdentifier);
        }
    }
}
