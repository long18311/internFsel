using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ServerSignalData.Models;
using ServerSignalData.repositories.IRepon;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Server.Hubs
{
    public class SignalHub : Hub
    {
        private static Dictionary<Guid, string> connectedClients = new Dictionary<Guid, string>();
        private readonly IMessageRepon _messageRepon;
        private readonly IUserRepon _userRepon;
        public SignalHub(IMessageRepon messageRepon, IUserRepon userRepon)
        {
            _messageRepon = messageRepon;
            _userRepon = userRepon;
        }
        /*[Authorize]*/
        public async Task SendMessage(Guid userSenderID, string message, Guid userReceiveID)
        {
            /*Guid userSenderID = connectedClients.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (userSenderID == Guid.Empty|| userSenderID == null) {
                string token = Context.GetHttpContext().Request.Query["access_token"];
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                // Lấy thông tin về người dùng từ token
                string userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                userSenderID = Guid.Parse(userId);
            }*/
            User userSender = await _userRepon.GetUserById(userSenderID);
            User userReceive = await _userRepon.GetUserById(userReceiveID);
            Message  message1 = new Message() {
                Id = Guid.NewGuid(),
                UserSenderid = userSender.Id,
                UserSender = userSender,
                time = DateTime.Now,
                UserReceiveid = userReceive.Id,
                Content = message
            };
            await _messageRepon.Create(message1);
            if (connectedClients.TryGetValue(userReceiveID, out string b)){
                await Clients.Clients(b).SendAsync("ReceiveMessage", userSender,userReceive);
            }
            await Clients.Clients(Context.ConnectionId).SendAsync("ReceiveMessage", userSender, userReceive);
        }
        public async Task JoinChat(Guid userid)
        {
            connectedClients[userid] = Context.ConnectionId;  
        }
    }
}
