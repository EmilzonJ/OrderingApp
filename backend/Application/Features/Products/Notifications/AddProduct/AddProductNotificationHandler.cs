using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Notifications.AddProduct
{
    public class AddProductNotificationHandler : INotificationHandler<AddProductNotification>
    {
        // private readonly IHubContext<Product> _hubContext;
        
        public Task Handle(AddProductNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("AddProductNotificationHandler: " + notification.Product.Name);
            
            return Task.CompletedTask;
        }
    }
}