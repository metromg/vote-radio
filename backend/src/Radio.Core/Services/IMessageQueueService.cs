using System;
using Radio.Core.Services.Messaging;

namespace Radio.Core.Services
{
    public interface IMessageQueueService
    {
        void Send(MessageBase message);

        IObservable<MessageBase> Receive();
    }
}
