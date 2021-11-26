using System;

namespace Common.Entities
{
    public interface IAggregate
    {
        bool Deleted { get; }
        DateTime Updated { get; set; }
        string UpdatedById { get; set; }
        DateTime Created { get; set; }
        string CreatedById { get; set; }
    }

    public interface IAggregate<out TPKey> : IAggregate
    {
        TPKey Id { get; }
    }
}