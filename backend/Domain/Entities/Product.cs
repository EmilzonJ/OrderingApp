using System;
using Domain.Base;

namespace Domain.Entities
{
    public class Product : EntityBase<Guid>
    {
        protected Product()
        {
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }

        public static Product Create(
            Guid id,
            string name,
            decimal price,
            string size
        )
        {
            var product = new Product
            {
                Id = id,
                Name = name,
                Price = price,
                Size = size
            };

            return product;
        }
        
        public void Update(
            string name,
            decimal price,
            string size
        )
        {
            Name = name;
            Price = price;
            Size = size;
        }
    }
}