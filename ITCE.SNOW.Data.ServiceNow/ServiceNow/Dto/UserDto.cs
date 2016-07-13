using System.Runtime.Serialization;
using ITCE.SNOW.Domain;

namespace ITCE.SNOW.Data.ServiceNow.ServiceNow.Dto
{
    [DataContract]
    public class UserDto : User, IServiceNowDto
    {
        [DataMember(Name = "name")]
        public override string Name { get; set; }
        [DataMember(Name = "user_name")]
        public override string UserName { get; set; }
        [DataMember(Name = "company.name")]
        public override string Company { get; set; }

        public string Fields { get; } = "sysparm_fields=name,user_name,company.name";
    }
}
