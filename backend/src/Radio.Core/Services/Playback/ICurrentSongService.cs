using System.Threading.Tasks;
using Radio.Core.Domain.Voting.Objects;

namespace Radio.Core.Services.Playback
{
    public interface ICurrentSongService
    {
        Task UpdateOrCreateAsync(SongWithVoteCount song);
    }
}
