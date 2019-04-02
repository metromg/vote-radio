using System;

namespace Radio.Core.Services.Messaging
{
    public class VoteMessage : IMessage
    {
        public Guid SongId { get; set; }
    }
}
