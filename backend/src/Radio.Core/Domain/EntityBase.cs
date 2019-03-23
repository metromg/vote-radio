using System;

namespace Radio.Core.Domain
{
    public class EntityBase
    {
        public EntityBase()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
