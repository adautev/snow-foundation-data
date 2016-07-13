using System.Collections.Generic;

namespace ITCE.SNOW.Domain.Validation.Concrete
{
    public class ServicePortalRequestValidationSummary : IValidationSummary<ServicePortalRequest>
    {
        public string Name { get; set; }
        public List<ValidationMessage<ServicePortalRequest>> Messages { get; } = new List<ValidationMessage<ServicePortalRequest>>();
    }
}
