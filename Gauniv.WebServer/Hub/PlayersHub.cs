// File: Hubs/PlayersHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Hubs
{
    public class PlayersHub : Hub
    {
        // For real-time status updates, you might implement methods like:
        public async Task UpdateStatus(string status)
        {
            // Broadcast status update to all connected clients.
            await Clients.All.SendAsync("ReceiveStatusUpdate", Context.User.Identity.Name, status);
        }
    }
}
