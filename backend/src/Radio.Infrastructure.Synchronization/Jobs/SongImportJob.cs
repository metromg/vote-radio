using System;
using System.Threading.Tasks;
using FluentScheduler;
using Microsoft.Extensions.Logging;
using Radio.Core;
using Radio.Infrastructure.Synchronization.Services;

namespace Radio.Infrastructure.Synchronization.Jobs
{
    public class SongImportJob : IJob
    {
        private readonly IUnitOfWorkFactory<ISongImportService, ILogger> _unitOfWorkFactory;

        public SongImportJob(IUnitOfWorkFactory<ISongImportService, ILogger> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Execute()
        {
            ExecuteAsync().GetAwaiter().GetResult();
        }

        private async Task ExecuteAsync()
        {
            using (var unit = _unitOfWorkFactory.Begin())
            {
                try
                {
                    await unit.Dependent.ImportAsync();
                }
                catch (Exception ex)
                {
                    unit.Dependent2.LogCritical("Unhandled exception in ImportSongsJob. {0}", ex);
                }
            }
        }
    }
}
