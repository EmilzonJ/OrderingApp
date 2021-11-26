using System;
using Domain.Base;

namespace Domain.Entities
{
    public class Category : EntityBase<Guid>
    {
        protected Category()
        {
        }

        public string Name { get; set; }

        public static Category Create(
            Guid id,
            string name
        )
        {
            return new Category
            {
                Id = id,
                Name = name,
            };
        }
    }
}