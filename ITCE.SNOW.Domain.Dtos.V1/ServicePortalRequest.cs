using System.Runtime.Serialization;
using ITCE.SNOW.Domain;

namespace ITCE.SNOW.Dtos.V1
{
    public class ServicePortalRequest : DomainModelBase
    {
        [DataMember(Name = "Request  Name")]
        public string Name { get; set; }
        [DataMember(Name = "Related Business Service")]
        public string BusinessService { get; set; }
        [DataMember(Name = "Self-service portal sub category")]
        public string SubCategory { get; set; }
        [DataMember(Name = "Self-service portal category")]
        public string Category { get; set; }
        [DataMember(Name = "Approver (Person, or role, e.g. Line Manager)")]
        public User Approver { get; set; }
        [DataMember(Name = "Implementer Group")]
        public string AssignmentGroup { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
    }
}
