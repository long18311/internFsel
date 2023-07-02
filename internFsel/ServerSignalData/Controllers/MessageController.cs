using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerSignalData.Models;
using ServerSignalData.repositories.IRepon;
using ServerSignalData.ViewModel.Message;
using ServerSignalData.ViewModel.User;
using System.Collections.Generic;
using System.Security.Claims;

namespace ServerSignalData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUserRepon _userRepon;
        private readonly IMessageRepon _messageRepon;
        public MessageController(IMessageRepon messageRepon, IUserRepon userRepon)
        {
            _userRepon = userRepon;
            _messageRepon = messageRepon;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get(Guid userReceiveid)
        {
            string userId = User.FindFirstValue(ClaimTypes.Email);
            Guid UserSenderid = Guid.Parse(userId);
            List<Message> result = await _messageRepon.GetList(UserSenderid, userReceiveid);
            if(result == null)
            {
                return Ok(null);
            }
            List<MessageView> messageViews = new List<MessageView>();
            result.Sort((obj1, obj2) => obj1.time.CompareTo(obj2.time));
            User userReceive = await _userRepon.GetUserById(userReceiveid);

            foreach (var message in result)
            {
                messageViews.Add(new MessageView() {
                    Id = message.Id,
                    Content = message.Content,
                    time = message.time,
                    UserReceiveid = message.UserReceiveid,
                    UserSenderid = message.UserSenderid                    
                });
            }
            return Ok(result);
        }
        /*[HttpPost]
        public async Task<IActionResult> Get(FilterMessage filterMessage)
        {
            List<Message> result = await _messageRepon.GetList(filterMessage.UserSenderid, filterMessage.UserReceiveid);
            if (result == null)
            {
                return Ok(null);
            }
            List<MessageView> messageViews = new List<MessageView>();
            result.Sort((obj1, obj2) => obj1.time.CompareTo(obj2.time));
            User userReceive = await _userRepon.GetUserById(filterMessage.UserReceiveid);

            foreach (var message in result)
            {
                messageViews.Add(new MessageView()
                {
                    Id = message.Id,
                    Content = message.Content,
                    time = message.time,
                    UserReceiveid = message.UserReceiveid,
                    UserSenderid = message.UserSenderid
                });
            }
            return Ok(result);
        }*/
    }
}
