using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;


namespace Hubs
{
    [Authorize]
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage", string.Empty, $"{Context.User.Identity.Name} joined!");
            await base.OnConnectedAsync();
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}