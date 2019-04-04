using System.Threading.Tasks;
using Radio.Core.Domain.Voting.Objects;

namespace Radio.Core.Services.Playback
{
    public interface ICurrentSongService
    {
        Task CreateOrUpdateAsync(SongWithVoteCount song);
    }
}
