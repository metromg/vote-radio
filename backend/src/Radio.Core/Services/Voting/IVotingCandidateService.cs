using System.Collections.Generic;
using System.Threading.Tasks;
using Radio.Core.Domain.MasterData.Model;

namespace Radio.Core.Services.Voting
{
    public interface IVotingCandidateService
    {
        Task UpdateOrCreateAsync(IEnumerable<Song> songs);
    }
}
