using System;
using Radio.Core.Services.Messaging;

namespace Radio.Core.Services
{
    public interface IMessageQueueService
    {
        void Send(IMessage message);

        IObservable<IMessage> Receive();
    }
}
