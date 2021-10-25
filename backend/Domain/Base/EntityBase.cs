using System;

namespace Domain.Base
{
    #region Interfaces

    public interface IAuditEntity
    {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        string UpdatedBy { get; set; }
        bool IsDeleted { get; set; }
    }
    
    public interface IEntityBase<out TPKey> : IAuditEntity
    {
        TPKey Id { get; }
    }

    #endregion Interfaces

    #region Abstract Classes

    public abstract class EntityBase<TPKey> : IEntityBase<TPKey>
    {
        public virtual TPKey Id { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual bool IsDeleted { get; set; }
    }

    #endregion Abstract Classes
}