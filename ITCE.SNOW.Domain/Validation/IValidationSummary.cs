using System.Collections.Generic;

namespace ITCE.SNOW.Domain.Validation
{
    public interface IValidationSummary<T> where T: DomainModelBase, new()
    {
        string Name { get; set; }
        List<ValidationMessage<T>> Messages
        { 
            get;
        }
    }
}
