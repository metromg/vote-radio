using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.MasterData.Model;

namespace Radio.Infrastructure.DbAccess.Domain.MasterData
{
    public class SongRepository : Repository<Song>, ISongRepository
    {
        public SongRepository(DbSet<Song> set)
            : base(set)
        {
        }

        public Task<Song[]> GetRandomAsync(int take)
        {
            return GetQuery()
                .OrderBy(_ => Guid.NewGuid())
                .Take(take)
                .ToArrayAsync();
        }
    }
}
