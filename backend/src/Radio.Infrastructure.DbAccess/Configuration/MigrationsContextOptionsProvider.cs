using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Radio.Core;

namespace Radio.Infrastructure.DbAccess.Configuration
{
    public class MigrationsContextOptionsProvider : IContextOptionsProvider
    {
        private readonly IOptions<EnvironmentOptions> _environmentOptions;

        public MigrationsContextOptionsProvider(IOptions<EnvironmentOptions> environmentOptions)
        {
            _environmentOptions = environmentOptions;
        }

        public void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql(_environmentOptions.Value.MigrationsDbConnectionString);
        }
    }
}
