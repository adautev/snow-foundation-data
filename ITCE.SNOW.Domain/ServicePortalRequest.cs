namespace ITCE.SNOW.Domain
{
    public class ServicePortalRequest : DomainModelBase
    {
        public virtual string Name { get; set; }
        public virtual string BusinessService { get; set; }
        public virtual string SubCategory { get; set; }
        public virtual string Category { get; set; }
        public virtual User Approver { get; set; }
        public virtual string AssignmentGroup { get; set; }
        public virtual string Description { get; set; }
        public virtual string Country { get; set; }
    }
}
