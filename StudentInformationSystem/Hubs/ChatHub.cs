using Microsoft.AspNetCore.SignalR;

namespace StudentInformationSystem.Web.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string department, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, department, message);
        }
    }
}
