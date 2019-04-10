using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Radio.Core;
using Radio.Core.Domain.Playback;
using Radio.Core.Domain.Voting;
using Radio.Core.Services;
using Radio.Infrastructure.Api.External.Dtos;

namespace Radio.Infrastructure.Api.External.Hubs
{
    public class RadioHubMessageDispatcher
    {
        private readonly IUnitOfWorkFactory<ICurrentSongRepository, IVotingCandidateRepository, IMapper> _unitOfWorkFactory;

        public RadioHubMessageDispatcher(IUnitOfWorkFactory<ICurrentSongRepository, IVotingCandidateRepository, IMapper> unitOfWorkFactory)
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
                case Message.DisableVotingMessage:
                    return DispatchDisableVotingMessage(clients);
                default:
                    throw new ArgumentOutOfRangeException(nameof(message));
            }
        }

        private async Task DispatchNextSongMessage(IClientProxy clients)
        {
            using (var unit = _unitOfWorkFactory.Begin())
            {
                var currentSong = await GetCurrentSongAsync(unit);
                var votingCandidates = await GetVotingCandidatesAsync(unit);

                await clients.SendAsync("NextSong", currentSong, votingCandidates);
            }
        }

        private async Task DispatchVotingMessage(IClientProxy clients)
        {
            using (var unit = _unitOfWorkFactory.Begin())
            {
                var votingCandidates = await GetVotingCandidatesAsync(unit);

                await clients.SendAsync("Vote", votingCandidates);
            }
        }

        private async Task DispatchDisableVotingMessage(IClientProxy clients)
        {
            await clients.SendAsync("DisableVoting");
        }

        private static async Task<CurrentSongDto> GetCurrentSongAsync(IUnitOfWork<ICurrentSongRepository, IVotingCandidateRepository, IMapper> unit)
        {
            var currentSong = await unit.Dependent.GetOrDefaultAsync();

            return unit.Dependent3.Map<CurrentSongDto>(currentSong);
        }

        private static async Task<VotingCandidateDto[]> GetVotingCandidatesAsync(IUnitOfWork<ICurrentSongRepository, IVotingCandidateRepository, IMapper> unit)
        {
            var votingCandidates = await unit.Dependent2.GetWithVoteCountAsync();

            return votingCandidates.Select(unit.Dependent3.Map<VotingCandidateDto>).ToArray();
        }
    }
}
