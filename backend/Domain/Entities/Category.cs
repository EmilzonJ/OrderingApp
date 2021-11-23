using System;
using Domain.Base;

namespace Domain.Entities
{
    public class Category : EntityBase<Guid>
    {
        public string Name { get; set; }
        public Guid ProductForeignKey { get; set; }
        public Product Product { get; set; }
    }
}