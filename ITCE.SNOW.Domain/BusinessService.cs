namespace ITCE.SNOW.Domain
{
    public class BusinessService : DomainModelBase
    {
        public virtual string Name { get; set; }
        public virtual string AssignmentGroup { get; set; }
        public virtual string BusinessCriticality { get; set; }
        public virtual User Manager { get; set; }
        public virtual string Country { get; set; }
        public virtual string Comments { get; set; }
    }
}
