namespace Pwipper.Core.Domain
{
    public class User : DomainObject<long>
    {
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
    }
}