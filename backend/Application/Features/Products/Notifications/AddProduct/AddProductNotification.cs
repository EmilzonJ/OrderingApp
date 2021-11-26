using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Notifications.AddProduct
{
    public class AddProductNotification : INotification
    {
        public Product Product { get; set; }

        public AddProductNotification(Product product)
        {
            Product = product;
        }
    }
}