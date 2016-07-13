using System.Runtime.Serialization;
using ITCE.SNOW.Domain;

namespace ITCE.SNOW.Dtos.V1
{
    public class BusinessService : DomainModelBase
    {
        public string Name { get; set; }
        [DataMember(Name = "Assignment Group US")]
        public string AssignmentGroupUs { get; set; }
        [DataMember(Name = "Assignment Group GR")]
        public string AssignmentGroupGr { get; set; }
        [DataMember(Name = "Business Criticality US")]
        public string BusinessCriticalityUs { get; set; }
        [DataMember(Name = "Business Criticality GR")]
        public string BusinessCriticalityGr { get; set; }
        [DataMember(Name = "Managed By US")]
        public User ManagerUs { get; set; }
        [DataMember(Name = "Managed By GR")]
        public User ManagerGr { get; set; }
        public string Country { get; set; }
        public string Comments { get; set; }
    }
}
