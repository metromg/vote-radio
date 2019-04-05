using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Radio.Core;
using Radio.Core.Domain.Voting;
using Radio.Core.Services;
using Radio.Infrastructure.Api.External.Dtos;

namespace Radio.Infrastructure.Api.External.Hubs
{
    public class RadioHubMessageDispatcher
    {
        private readonly IUnitOfWorkFactory<IVotingCandidateRepository, IMapper> _unitOfWorkFactory;

        public RadioHubMessageDispatcher(IUnitOfWorkFactory<IVotingCandidateRepository, IMapper> unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public Task Dispatch(Message message, IClientProxy clients)
        {
            switch (message)
            {
                case Message.NextSongMessage:
                    return DispatchNextSongMessage(clients);
                case Message.VotingMessage:
                    return DispatchVotingMessage(clients);
                default:
                    throw new ArgumentOutOfRangeException(nameof(message));
            }
        }

        private async Task DispatchNextSongMessage(IClientProxy clients)
        {
            await clients.SendAsync("DisableVoting");
            await Task.Delay(TimeSpan.FromSeconds(Constants.App.TIME_IN_SECONDS_BEFORE_END_OF_CURRENT_SONG_WHEN_REQUESTING_NEXT_SONG));

            using (var unit = _unitOfWorkFactory.Begin())
            {
                // TODO: Load next song
                var votingCandidates = await GetVotingCandidatesResult(unit);

                await clients.SendAsync("NextSong", votingCandidates);
            }
        }

        private async Task DispatchVotingMessage(IClientProxy clients)
        {
            using (var unit = _unitOfWorkFactory.Begin())
            {
                var votingCandidates = await GetVotingCandidatesResult(unit);

                await clients.SendAsync("Vote", votingCandidates);
            }
        }

        private static async Task<VotingCandidateDto[]> GetVotingCandidatesResult(IUnitOfWork<IVotingCandidateRepository, IMapper> unit)
        {
            var votingCandidates = await unit.Dependent.GetWithVoteCountAsync();

            return votingCandidates.Select(unit.Dependent2.Map<VotingCandidateDto>).ToArray();
        }
    }
}
