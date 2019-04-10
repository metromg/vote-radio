using Microsoft.Extensions.Options;

namespace Radio.Core
{
    public class EnvironmentOptions
    {
        public const string NAME = "Environment";

        public string DbConnectionString { get; set; }

        public string MigrationsDbConnectionString { get; set; }

        public string MessagingHost { get; set; }

        public string DataDirectoryPath { get; set; }
    }

    public static class EnvironmentOptionsMonitorExtensions
    {
        public static EnvironmentOptions Current(this IOptionsMonitor<EnvironmentOptions> monitor)
        {
            return monitor.Get(EnvironmentOptions.NAME);
        }
    }
}
