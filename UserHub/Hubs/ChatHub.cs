using Authorization;
using Authorization.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new Dictionary<string, string>();
        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            Users.Add(Context.ConnectionId, username);
            var userList = Users.Values.ToList();
            await Clients.All.SendAsync("ReceiveMessage", string.Empty, $"{username} joined!");
            await Clients.All.SendAsync("ReceiveUserUpdate", userList);
            await base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            await SendMessage(string.Empty, $"{username} left!");
            Users.Remove(Context.ConnectionId);
            var userList = Users.Values.ToList();
            await Clients.All.SendAsync("ReceiveUserUpdate", userList);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}