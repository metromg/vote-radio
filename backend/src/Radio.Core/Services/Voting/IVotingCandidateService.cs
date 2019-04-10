using System.Collections.Generic;
using System.Threading.Tasks;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.Voting.Model;

namespace Radio.Core.Services.Voting
{
    public interface IVotingCandidateService
    {
        Task<VotingCandidate[]> UpdateOrCreateAsync(IEnumerable<Song> songs);
    }
}
