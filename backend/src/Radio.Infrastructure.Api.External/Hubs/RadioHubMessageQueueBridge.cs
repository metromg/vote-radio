using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Radio.Core.Services;

namespace Radio.Infrastructure.Api.External.Hubs
{
    public class RadioHubMessageQueueBridge
    {
        private readonly IMessageQueueService _messageQueueService;
        private readonly RadioHubMessageDispatcher _radioHubMessageDispatcher;
        private readonly object _connectedIdsLockPad = new object();
        private readonly List<string> _connectedIds = new List<string>();
        private IDisposable _messageQueueSubscription;
        private IClientProxy _allClientProxy;

        public RadioHubMessageQueueBridge(IMessageQueueService messageQueueService, RadioHubMessageDispatcher radioHubMessageDispatcher)
        {
            _messageQueueService = messageQueueService;
            _radioHubMessageDispatcher = radioHubMessageDispatcher;
        }

        public void Connecting(string callerConnectionId, IClientProxy allClients)
        {
            _allClientProxy = allClients;

            bool hasOne;
            lock (_connectedIdsLockPad)
            {
                _connectedIds.Add(callerConnectionId);
                hasOne = _connectedIds.Count == 1;
            }

            if (hasOne)
            {
                _messageQueueSubscription = _messageQueueService.Receive()
                    .Subscribe(message => _radioHubMessageDispatcher.Dispatch(message, _allClientProxy));
            }
        }

        public void Disconnecting(string callerConnectionId, IClientProxy allClients)
        {
            _allClientProxy = allClients;

            bool hasNone;
            lock (_connectedIdsLockPad)
            {
                _connectedIds.Remove(callerConnectionId);
                hasNone = _connectedIds.Count == 0;
            }

            if (hasNone)
            {
                _messageQueueSubscription.Dispose();
                _allClientProxy = null;
            }
        }
    }
}
