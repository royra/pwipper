namespace Pwipper.Core.Domain
{
    public class DomainObject<TId>
    {
        public virtual TId Id { get; set; }
    }
}
