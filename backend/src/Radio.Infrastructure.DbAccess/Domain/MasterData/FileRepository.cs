using Microsoft.EntityFrameworkCore;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.MasterData.Model;

namespace Radio.Infrastructure.DbAccess.Domain.MasterData
{
    public class FileRepository : Repository<File>, IFileRepository
    {
        public FileRepository(DbSet<File> set)
            : base(set)
        {
        }
    }
}
