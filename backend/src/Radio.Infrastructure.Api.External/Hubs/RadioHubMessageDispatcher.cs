using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Radio.Core.Services.Messaging;

namespace Radio.Infrastructure.Api.External.Hubs
{
    public class RadioHubMessageDispatcher
    {
        public Task Dispatch(IMessage message, IClientProxy clients)
        {
            if (message is VoteMessage voteMessage)
            {
                return Dispatch(voteMessage, clients);
            }

            throw new ArgumentOutOfRangeException(nameof(message));
        }

        private Task Dispatch(VoteMessage voteMessage, IClientProxy clients)
        {
            return clients.SendAsync("Vote", voteMessage.SongId);
        }
    }
}
