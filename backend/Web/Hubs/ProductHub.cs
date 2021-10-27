using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs
{
    public class ProductHub : Hub
    {
        public async Task Send(Product product)
        {
            await Clients.All.SendAsync("ReceiveProducts", product);
        }
    }
}