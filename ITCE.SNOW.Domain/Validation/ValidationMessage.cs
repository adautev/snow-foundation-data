namespace ITCE.SNOW.Domain.Validation
{
    public class ValidationMessage<T> where T : DomainModelBase, new()
    {
        public T Reference { get; set; }
        public string Message;
    }
}
