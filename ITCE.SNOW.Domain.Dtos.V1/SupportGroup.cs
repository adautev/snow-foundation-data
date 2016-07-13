using System.Runtime.Serialization;
using ITCE.SNOW.Domain;

namespace ITCE.SNOW.Dtos.V1
{
    public class SupportGroup : DomainModelBase
    {
        [DataMember(Name = "Group Name")]
        public string Name { get; set; }
        [DataMember(Name = "Manager Name")]
        public User Manager { get; set; }
        [DataMember(Name= "Group Email")]
        public string Email { get; set; }
        [DataMember(Name = "Group Members (one row for each member)")]
        public string Member { get; set; }

        public string Country { get; set; }
    }
}
