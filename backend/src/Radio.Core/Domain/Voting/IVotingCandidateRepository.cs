﻿using System;
using System.Threading.Tasks;
using Radio.Core.Domain.Voting.Model;
using Radio.Core.Domain.Voting.Objects;

namespace Radio.Core.Domain.Voting
{
    public interface IVotingCandidateRepository : IRepository<VotingCandidate>
    {
        Task<SongWithVoteCount[]> GetWithVoteCountAsync();

        Task<SongWithVoteCount> GetWinnerOfVotingAsync();

        Task<VotingCandidate> GetBySongAsync(Guid songId);
    }
}
