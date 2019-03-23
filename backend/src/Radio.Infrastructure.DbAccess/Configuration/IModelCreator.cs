using Microsoft.EntityFrameworkCore;

namespace Radio.Infrastructure.DbAccess.Configuration
{
    public interface IModelCreator
    {
        void OnModelCreating(ModelBuilder builder);
    }
}
