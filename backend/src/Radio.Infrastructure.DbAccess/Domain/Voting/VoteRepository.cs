using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Radio.Core.Domain.Voting;
using Radio.Core.Domain.Voting.Model;

namespace Radio.Infrastructure.DbAccess.Domain.Voting
{
    public class VoteRepository : Repository<Vote>, IVoteRepository
    {
        public VoteRepository(DbSet<Vote> set)
            : base(set)
        {
        }

        public Task<Vote> GetByUserIdentifierOrDefaultAsync(Guid userIdentifier)
        {
            return GetQuery()
                .FirstOrDefaultAsync(v => v.UserIdentifier == userIdentifier);
        }
    }
}
