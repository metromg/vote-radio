using System.Threading.Tasks;
using Radio.Core.Domain.Playback.Model;

namespace Radio.Core.Domain.Playback
{
    public interface ICurrentSongRepository : IRepository<CurrentSong>
    {
        Task<CurrentSong> GetOrDefaultAsync();
    }
}
