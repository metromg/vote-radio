using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Radio.Infrastructure.Synchronization.Services
{
    public class SongImportService : ISongImportService
    {
        private readonly ILogger _logger;

        public SongImportService(ILogger logger)
        {
            _logger = logger;
        }

        public Task ImportAsync()
        {
            _logger.LogInformation("MUHA");

            return Task.CompletedTask;
        }
    }
}
