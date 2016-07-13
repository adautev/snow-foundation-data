using ITCE.SNOW.Domain;

namespace ITCE.SNOW.Data.ServiceNow.ServiceNow.Dto
{
    class BusinessServiceDto : BusinessService, IServiceNowDto
    {
        public string Fields { get; } = "sysparm_fields=name";
    }
}
