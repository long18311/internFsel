using Microsoft.AspNetCore.SignalR;
using Server.Models;

namespace Server.Hubs
{
    public class SignalHub : Hub
    {
        
        public void BroadcastEmployee(Employee employee)
        {
            Clients.All.SendAsync("ReceiveEmployee", employee);
        }
        public void BroadcastMessage(string user, string message)
        {
            Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public void BroadcastMessage(UserMessage userMessage)
        {
            Clients.All.SendAsync("ReceiveMessage", userMessage.user, userMessage.message);
        }
    }
}
