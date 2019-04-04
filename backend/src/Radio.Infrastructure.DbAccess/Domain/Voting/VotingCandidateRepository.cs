using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Radio.Core;
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
            return GetWithVoteCount().ToArrayAsync();
        }

        public Task<SongWithVoteCount> GetWinnerOfVotingAsync()
        {
            return GetWithVoteCount()
                .OrderByDescending(c => c.VoteCount)
                .FirstAsync();
        }

        public Task<VotingCandidate> GetBySongAsync(Guid songId)
        {
            return GetQuery().FirstAsync(c => c.SongId == songId);
        }

        protected override IQueryable<VotingCandidate> GetQuery()
        {
            // There will always be only 3 candidates in the database
            return base.GetQuery().Take(Constants.App.NUMBER_OF_VOTING_CANDIDATES);
        }

        private IQueryable<SongWithVoteCount> GetWithVoteCount()
        {
            return GetQuery()
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new SongWithVoteCount
                {
                    Song = c.Song,
                    VoteCount = c.Votes.Count
                });
        }
    }
}
