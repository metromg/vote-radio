using System.Threading.Tasks;
using Radio.Core.Domain.Playback.Model;
using Radio.Core.Domain.Voting.Objects;

namespace Radio.Core.Services.Playback
{
    public interface ICurrentSongService
    {
        Task<CurrentSong> UpdateOrCreateAsync(SongWithVoteCount song);
    }
}
