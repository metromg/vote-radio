using Microsoft.EntityFrameworkCore;

namespace Radio.Infrastructure.DbAccess.Configuration
{
    public interface IContextOptionsProvider
    {
        void OnConfiguring(DbContextOptionsBuilder builder);
    }
}
