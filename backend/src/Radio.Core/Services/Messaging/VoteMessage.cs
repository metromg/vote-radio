using System;

namespace Radio.Core.Services.Messaging
{
    public class VoteMessage : MessageBase
    {
        public Guid SongId { get; set; }
    }
}
