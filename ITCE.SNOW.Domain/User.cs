namespace ITCE.SNOW.Domain
{
    public class User : DomainModelBase
    {
        public User() { }

        public User(string name)
        {
            // ReSharper disable once VirtualMemberCallInContructor
            Name = name;
        }

        public virtual string Name { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Company { get; set; }
    }
}