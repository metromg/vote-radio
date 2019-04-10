using System;
using System.Threading.Tasks;
using Radio.Core.Domain.Voting.Objects;

namespace Radio.Core.Services
{
    public interface IVotingFinisher
    {
        Task<SongWithVoteCount> CollectResultAndLockAsync();

        Task ApplyResultAsync(Guid votingResultSongId);
    }
}
