using System;
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

        public Task<SongWithVoteCount[]> GetWithVoteCountAsync()
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

        public Task<VotingCandidate> GetBySongAsync(Guid songId)
        {
            return GetQuery()
                .FirstAsync(candidate => candidate.SongId == songId);
        }

        protected override IQueryable<VotingCandidate> GetQuery()
        {
            // There will always be exactly 3 candidates in the database
            return base.GetQuery().Take(3);
        }
    }
}
