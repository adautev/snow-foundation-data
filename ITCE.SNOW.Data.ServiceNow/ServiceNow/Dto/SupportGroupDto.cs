using System.Runtime.Serialization;
using ITCE.SNOW.Domain;

namespace ITCE.SNOW.Data.ServiceNow.ServiceNow.Dto
{
    [DataContract]
    public class SupportGroupDto : SupportGroup
    {
        public override string Name { get; set; }
        public string Fields { get; } = "sysparm_fields=name";
    }
}
