using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    public interface IProductHub
    {
        Task ReceiveProduct(Product product);
        Task UpdateProduct(Product product);
    }
    
    public class ProductHub : Hub<IProductHub>
    {
        public async Task SendProduct(Product product)
        {
            await Clients.All.ReceiveProduct(product);
        }
        
    }
}