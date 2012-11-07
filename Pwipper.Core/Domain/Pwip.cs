using System;

namespace Pwipper.Core.Domain
{
    public class Pwip : DomainObject<long>
    {
        public virtual DateTime Time { get; set; }
        public virtual string Text { get; set; }
        public virtual User Author { get; set; }
    }
}
