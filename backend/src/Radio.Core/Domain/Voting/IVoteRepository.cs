using System;
using System.Threading.Tasks;
using Radio.Core.Domain.Voting.Model;

namespace Radio.Core.Domain.Voting
{
    public interface IVoteRepository : IRepository<Vote>
    {
        Task<Vote> GetByUserIdentifierOrDefaultAsync(Guid userIdentifier);
    }
}
