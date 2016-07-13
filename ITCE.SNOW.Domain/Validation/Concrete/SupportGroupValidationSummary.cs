using System.Collections.Generic;

namespace ITCE.SNOW.Domain.Validation.Concrete
{
    public class SupportGroupValidationSummary : IValidationSummary<SupportGroup>
    {
        public string Name { get; set; }
        public List<ValidationMessage<SupportGroup>> Messages { get; } = new List<ValidationMessage<SupportGroup>>();
    }
}
