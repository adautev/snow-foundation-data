using System.Runtime.Serialization;
using ITCE.SNOW.Domain;

namespace ITCE.SNOW.Data.ServiceNow.ServiceNow.Dto
{
    [DataContract]
    public class ServicePortalRequestDto : ServicePortalRequest, IServiceNowDto
    {
        [DataMember(Name = "name")]
        public override string Name { get; set; }
        [DataMember(Name = "category.title")]
        public override string Category { get; set; }

        public string Fields { get; } = "sysparm_fields=name,short_description,description,category.title";
    }

    public interface IServiceNowDto
    {
        string Fields { get;  }
    }
}
