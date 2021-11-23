using System;
using Domain.Base;

namespace Domain.Entities
{
    public class Product : EntityBase<Guid>
    {
        public Product() { }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public Category Category { get; set; }
    }
}