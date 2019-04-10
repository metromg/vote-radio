using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Radio.Core;
using Radio.Core.Services;

namespace Radio.Infrastructure.Api.Internal.Controllers
{
    [Route("next")]
    public class NextSongController : Controller
    {
        private readonly IVotingFinisher _votingFinisher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IRootLifetimeScopeProvider _rootLifetimeScopeProvider;

        public NextSongController(IVotingFinisher votingFinisher, IUnitOfWork unitOfWork, IMessageQueueService messageQueueService, IRootLifetimeScopeProvider rootLifetimeScopeProvider)
        {
            _votingFinisher = votingFinisher;
            _unitOfWork = unitOfWork;
            _messageQueueService = messageQueueService;
            _rootLifetimeScopeProvider = rootLifetimeScopeProvider;
        }

        [HttpGet]
        public async Task<string> NextAsync()
        {
            var votingResult = await _votingFinisher.CollectResultAndLockAsync();
            await _unitOfWork.CommitAsync();

            _messageQueueService.Send(Message.DisableVotingMessage);

            Response.OnCompleted(() => OnResponseCompleted(votingResult.Song.Id, _rootLifetimeScopeProvider));

            return votingResult.Song.FileName;
        }

        private static async Task OnResponseCompleted(Guid votingResultSongId, IRootLifetimeScopeProvider rootLifetimeScopeProvider)
        {
            await Task.Delay(TimeSpan.FromSeconds(Constants.App.TIME_BEFORE_START_OF_NEXT_SONG_IN_SECONDS + Constants.App.CROSSFADE_DURATION_IN_SECONDS));

            var unitOfWorkFactory = rootLifetimeScopeProvider.Get().Resolve<IUnitOfWorkFactory<IVotingFinisher, IMessageQueueService>>();
            using (var unit = unitOfWorkFactory.Begin())
            {
                await unit.Dependent.ApplyResultAsync(votingResultSongId);
                await unit.CommitAsync();

                unit.Dependent2.Send(Message.NextSongMessage);
            }
        }
    }
}
