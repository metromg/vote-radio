using System;

namespace Radio.Core
{
    public interface IClock
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }

        DateTime Today { get; }
    }
}
