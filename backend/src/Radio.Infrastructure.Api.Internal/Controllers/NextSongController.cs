using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Radio.Core;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.Voting;
using Radio.Core.Domain.Voting.Objects;
using Radio.Core.Services;
using Radio.Core.Services.Playback;
using Radio.Core.Services.Voting;

namespace Radio.Infrastructure.Api.Internal.Controllers
{
    [Route("next")]
    public class NextSongController : Controller
    {
        private readonly IVotingCandidateRepository _votingCandidateRepository;
        private readonly ISongRepository _songRepository;
        private readonly ICurrentSongService _currentSongService;
        private readonly IVotingCandidateService _votingCandidateService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageQueueService _messageQueueService;

        public NextSongController(IVotingCandidateRepository votingCandidateRepository, ISongRepository songRepository, ICurrentSongService currentSongService, IVotingCandidateService votingCandidateService, IUnitOfWork unitOfWork, IMessageQueueService messageQueueService)
        {
            _votingCandidateRepository = votingCandidateRepository;
            _songRepository = songRepository;
            _currentSongService = currentSongService;
            _votingCandidateService = votingCandidateService;
            _unitOfWork = unitOfWork;
            _messageQueueService = messageQueueService;
        }

        [HttpGet]
        public async Task<string> NextAsync()
        {
            var winnerOfVoting = await _votingCandidateRepository.GetWinnerOfVotingOrDefaultAsync();
            if (winnerOfVoting == null)
            {
                // This is the very first song request. Voting candidates don't exist yet.
                var randomSong = await _songRepository.GetRandomAsync(take: 1);
                winnerOfVoting = new SongWithVoteCount
                {
                    Song = randomSong.First(),
                    VoteCount = 0
                };
            }

            var newVotingCandidateSongs = await _songRepository.GetRandomAsync(take: Constants.App.NUMBER_OF_VOTING_CANDIDATES);

            await _currentSongService.UpdateOrCreateAsync(winnerOfVoting);
            await _votingCandidateService.UpdateOrCreateAsync(newVotingCandidateSongs);

            await _unitOfWork.CommitAsync();

            _messageQueueService.Send(Message.NextSongMessage);

            return winnerOfVoting.Song.FileName;
        }
    }
}
