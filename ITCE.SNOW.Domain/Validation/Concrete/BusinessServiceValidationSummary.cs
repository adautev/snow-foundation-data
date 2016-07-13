using System.Collections.Generic;

namespace ITCE.SNOW.Domain.Validation.Concrete
{
    public class BusinessServiceValidationSummary : IValidationSummary<BusinessService>
    {
        public string Name { get; set; }
        public List<ValidationMessage<BusinessService>> Messages { get; } = new List<ValidationMessage<BusinessService>>();
    }
}