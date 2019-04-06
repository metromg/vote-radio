using Microsoft.EntityFrameworkCore;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.MasterData.Model;

namespace Radio.Infrastructure.DbAccess.Domain.MasterData
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(DbSet<Image> set)
            : base(set)
        {
        }
    }
}
