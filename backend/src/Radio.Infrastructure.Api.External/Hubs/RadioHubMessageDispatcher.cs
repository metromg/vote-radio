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
                case Message.VotingMessage:
                    return DispatchVotingMessage(clients);
                default:
                    throw new ArgumentOutOfRangeException(nameof(message));
            }
        }

        private async Task DispatchVotingMessage(IClientProxy clients)
        {
            using (var unit = _unitOfWorkFactory.Begin())
            {
                var candidates = await unit.Dependent.GetWithVoteCountAsync();
                var result = candidates.Select(unit.Dependent2.Map<VotingCandidateDto>).ToArray();

                await clients.SendAsync("Vote", result);
            }
        }
    }
}
