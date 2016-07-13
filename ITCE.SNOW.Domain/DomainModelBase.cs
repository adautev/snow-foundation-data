using System;

namespace ITCE.SNOW.Domain
{
    public class DomainModelBase
    {
        public Guid Id { get; } = Guid.NewGuid();
        public dynamic Reference { get; set; }
    }
}
