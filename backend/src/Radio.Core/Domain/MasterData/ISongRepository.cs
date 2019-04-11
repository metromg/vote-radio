using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radio.Core.Domain.MasterData.Model;

namespace Radio.Core.Domain.MasterData
{
    public interface ISongRepository : IRepository<Song>
    {
        Task<Song[]> GetRandomAsync(int take);

        Song GetByFileNameOrDefault(string fileName);

        IEnumerable<Song> GetNextSongsToRemove(DateTimeOffset importDate, int batchSize);
    }
}
