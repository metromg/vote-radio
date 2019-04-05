using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Radio.Core;
using Radio.Core.Domain.Playback;
using Radio.Core.Domain.Playback.Model;

namespace Radio.Infrastructure.DbAccess.Domain.Playback
{
    public class CurrentSongRepository : Repository<CurrentSong>, ICurrentSongRepository
    {
        public CurrentSongRepository(DbSet<CurrentSong> set)
            : base(set)
        {
        }

        public Task<CurrentSong> GetOrDefaultAsync()
        {
            return GetQuery().FirstOrDefaultAsync();
        }

        protected override IQueryable<CurrentSong> GetQuery()
        {
            // There will always be only 1 current song in the database
            return base.GetQuery().Take(Constants.App.NUMBER_OF_CURRENT_SONGS);
        }
    }
}
