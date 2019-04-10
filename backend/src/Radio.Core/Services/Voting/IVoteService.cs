using System;
using System.Threading.Tasks;
using Radio.Core.Domain.Voting.Model;

namespace Radio.Core.Services.Voting
{
    public interface IVoteService
    {
        Task<Vote> UpdateOrCreateAsync(VotingCandidate votingCandidate, Guid userIdentifier);
    }
}
