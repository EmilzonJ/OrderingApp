using System;

namespace Common.Entities
{
    public abstract class Aggregate<TPKey> : IAggregate<TPKey>
    {
        public virtual TPKey Id { get; }
        
        public virtual bool Deleted { get; }
        
        public virtual DateTime Updated { get; set; }

        public virtual string UpdatedById { get; set; }
        
        public virtual DateTime Created { get; set; }
        
        public virtual string CreatedById { get; set; }
    }
}