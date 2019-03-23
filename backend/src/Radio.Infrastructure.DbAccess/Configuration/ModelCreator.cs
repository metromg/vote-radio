using Microsoft.EntityFrameworkCore;
using Radio.Infrastructure.DbAccess.Mapping;

namespace Radio.Infrastructure.DbAccess.Configuration
{
    public class ModelCreator : IModelCreator
    {
        public void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SongMapping());
        }
    }
}
