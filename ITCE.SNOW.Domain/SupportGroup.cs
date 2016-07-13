using System.Collections.Generic;

namespace ITCE.SNOW.Domain
{
    public class SupportGroup : DomainModelBase
    {
        public virtual string Country { get; set; }
        public virtual string Name { get; set; }
        public virtual User Manager { get; set; }
        public virtual string Email { get; set; }
        public virtual IEnumerable<User> Members { get; set; } = new List<User>();
    }
}
