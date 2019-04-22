using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Radio.Core.Domain.MasterData;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.Voting;
using Radio.Core.Domain.Voting.Objects;
using Radio.Core.Services.Playback;
using Radio.Core.Services.Voting;

namespace Radio.Core.Services
{
    public class VotingFinisher : IVotingFinisher
    {
        private readonly IVotingCandidateRepository _votingCandidateRepository;
        private readonly ISongRepository _songRepository;
        private readonly ICurrentSongService _currentSongService;
        private readonly IVotingCandidateService _votingCandidateService;
        private readonly ILogger _logger;

        public VotingFinisher(IVotingCandidateRepository votingCandidateRepository, ISongRepository songRepository, ICurrentSongService currentSongService, IVotingCandidateService votingCandidateService, ILogger logger)
        {
            _votingCandidateRepository = votingCandidateRepository;
            _songRepository = songRepository;
            _currentSongService = currentSongService;
            _votingCandidateService = votingCandidateService;
            _logger = logger;
        }

        public async Task<SongWithVoteCount> CollectResultAndLockAsync()
        {
            var votingResult = await _votingCandidateRepository.GetWinnerOfVotingWithVoteCountOrDefaultAsync();
            if (votingResult == null)
            {
                // If there are no voting candidates yet, we have to choose a random song.
                var randomSong = await _songRepository.GetRandomAsync(take: 1);
                votingResult = SongWithDefaultVoteCount(randomSong.First());
            }

            var currentVotingCandidates = _votingCandidateRepository.Get();
            foreach (var votingCandidate in currentVotingCandidates)
            {
                votingCandidate.IsActive = false;
            }

            _logger.LogInformation("The winner is {0}. Voting is locked now.", votingResult.Song.FileName);

            return votingResult;
        }

        public async Task ApplyResultAsync(Guid votingResultSongId)
        {
            var votingCandidateToApply = await _votingCandidateRepository.GetWithVoteCountBySongOrDefaultAsync(votingResultSongId);
            if (votingCandidateToApply == null)
            {
                // If there are no voting candidates yet, we have to construct a new song with default vote count
                var song = await _songRepository.GetByIdAsync(votingResultSongId);
                votingCandidateToApply = SongWithDefaultVoteCount(song);
            }

            var newVotingCandidateSongs = await _songRepository.GetRandomAsync(take: Constants.App.NUMBER_OF_VOTING_CANDIDATES);

            var currentSong = await _currentSongService.UpdateOrCreateAsync(votingCandidateToApply);
            await _votingCandidateService.UpdateOrCreateAsync(newVotingCandidateSongs);

            _logger.LogInformation("Changing current song to {0}. Estimated end is {1}", currentSong.Song.FileName, currentSong.EndsAtTime.ToString());
        }

        private static SongWithVoteCount SongWithDefaultVoteCount(Song song)
        {
            return new SongWithVoteCount
            {
                Song = song,
                VoteCount = 0,
                DisplayOrder = 0,
                IsActive = true
            };
        }
    }
}
