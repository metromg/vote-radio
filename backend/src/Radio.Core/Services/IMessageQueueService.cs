using System;

namespace Radio.Core.Services
{
    public interface IMessageQueueService
    {
        void Send(Message message);

        IObservable<Message> Receive();
    }
}
