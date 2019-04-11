using System;
using System.Collections.Generic;
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

        public Song GetByFileNameOrDefault(string fileName)
        {
            return GetQuery().FirstOrDefault(song => song.FileName == fileName);
        }

        public IEnumerable<Song> GetNextSongsToRemove(DateTimeOffset importDate, int batchSize)
        {
            return GetQuery()
                .Where(song => song.LastImportDate < importDate)
                .Take(batchSize);
        }
    }
}
