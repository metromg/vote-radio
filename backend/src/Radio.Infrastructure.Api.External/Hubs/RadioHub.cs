using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Radio.Infrastructure.Api.External.Hubs
{
    public class RadioHub : Hub
    {
        private readonly RadioHubMessageQueueBridge _radioHubMessageQueueBridge;

        public RadioHub(RadioHubMessageQueueBridge radioHubMessageQueueBridge)
        {
            _radioHubMessageQueueBridge = radioHubMessageQueueBridge;
        }

        public override Task OnConnectedAsync()
        {
            _radioHubMessageQueueBridge.Connecting(Context.ConnectionId, Clients.All);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _radioHubMessageQueueBridge.Disconnecting(Context.ConnectionId, Clients.All);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
