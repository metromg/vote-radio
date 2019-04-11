using System;
using FluentScheduler;
using Microsoft.Extensions.Logging;
using Radio.Infrastructure.Synchronization.Services;

namespace Radio.Infrastructure.Synchronization.Jobs
{
    public class SongImportJob : IJob
    {
        private readonly Func<ISongImportService> _songImportService;
        private readonly Func<ILogger> _logger;

        public SongImportJob(Func<ISongImportService> songImportService, Func<ILogger> logger)
        {
            _songImportService = songImportService;
            _logger = logger;
        }

        public void Execute()
        {
            try
            {
                _songImportService().Import();
            }
            catch (Exception ex)
            {
                _logger().LogCritical("Unhandled exception in SongImportJob. {0}", ex);
            }
        }
    }
}
