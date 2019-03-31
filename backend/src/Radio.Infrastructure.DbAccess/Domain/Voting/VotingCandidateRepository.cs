using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Radio.Core.Domain.Voting;
using Radio.Core.Domain.Voting.Model;
using Radio.Core.Domain.Voting.Objects;

namespace Radio.Infrastructure.DbAccess.Domain.Voting
{
    public class VotingCandidateRepository : Repository<VotingCandidate>, IVotingCandidateRepository
    {
        public VotingCandidateRepository(DbSet<VotingCandidate> set) 
            : base(set)
        {
        }

        public Task<SongWithVoteCount[]> GetWithVoteCount()
        {
            return GetQuery()
                .OrderBy(candidate => candidate.DisplayOrder)
                .Select(candidate => new SongWithVoteCount
                {
                    Song = candidate.Song,
                    VoteCount = candidate.Votes.Count
                })
                .ToArrayAsync();
        }
    }
}
