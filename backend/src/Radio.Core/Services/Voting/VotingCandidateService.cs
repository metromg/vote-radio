using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radio.Core.Domain.MasterData.Model;
using Radio.Core.Domain.Voting;

namespace Radio.Core.Services.Voting
{
    public class VotingCandidateService : IVotingCandidateService
    {
        private readonly IVotingCandidateRepository _votingCandidateRepository;

        public VotingCandidateService(IVotingCandidateRepository votingCandidateRepository)
        {
            _votingCandidateRepository = votingCandidateRepository;
        }

        public Task UpdateOrCreateAsync(IEnumerable<Song> songs)
        {
            RemoveExistingVotingCandidates();
            AddNewVotingCandidates(songs);

            return Task.CompletedTask;
        }

        private void RemoveExistingVotingCandidates()
        {
            // This will cascade delete all votes on the candidates
            var existingVotingCandidates = _votingCandidateRepository.Get();
            foreach (var candidate in existingVotingCandidates)
            {
                _votingCandidateRepository.Remove(candidate);
            }
        }

        private void AddNewVotingCandidates(IEnumerable<Song> songs)
        {
            foreach (var item in songs.Select((song, index) => new { song, index }))
            {
                var votingCandidate = _votingCandidateRepository.Create();
                _votingCandidateRepository.Add(votingCandidate);

                votingCandidate.Map(song: item.song, displayOrder: item.index);
            }
        }
    }
}
