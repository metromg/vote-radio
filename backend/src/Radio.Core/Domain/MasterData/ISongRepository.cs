using System.Threading.Tasks;
using Radio.Core.Domain.MasterData.Model;

namespace Radio.Core.Domain.MasterData
{
    public interface ISongRepository : IRepository<Song>
    {
        Task<Song[]> GetRandomAsync(int take);
    }
}
