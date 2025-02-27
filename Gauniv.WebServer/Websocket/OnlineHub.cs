using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace Gauniv.WebServer.Websocket
{
    public class OnlineHub : Hub
    {
        // In-memory dictionary to track online users.
        public static ConcurrentDictionary<string, bool> ConnectedUsers = new ConcurrentDictionary<string, bool>();

        public override async Task OnConnectedAsync()
        {
            // Retrieve the user id from the claims.
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers[userId] = true;
                await Clients.All.SendAsync("UserStatusChanged", userId, true);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers.TryRemove(userId, out _);
                await Clients.All.SendAsync("UserStatusChanged", userId, false);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
