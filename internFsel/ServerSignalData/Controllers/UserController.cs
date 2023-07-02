using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerSignalData.Models;
using ServerSignalData.repositories.IRepon;
using ServerSignalData.ViewModel.User;
using System.Security.Claims;

namespace ServerSignalData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepon _userRepon;
        public UserController(IUserRepon userRepon) {
            _userRepon = userRepon;
        }
        [HttpGet]
        [Route("GetUserLst")]
        [Authorize]
        public async Task<IActionResult> GetUserLst()
        {
            string userId = User.FindFirstValue(ClaimTypes.Email);
            Guid id = Guid.Parse(userId);
            List<User> result = await _userRepon.GetLstUser(id);
            List<UserView> userViews = new List<UserView>();
            foreach (User user in result)
            {
                userViews.Add(new UserView() {
                    Id = user.Id,
                    FullName = user.FullName,
                    Username = user.UserName,
                });
            }
            return Ok(userViews);
        }
        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            List<User> result = await _userRepon.GetLstUser(id);
            List<UserView> userViews = new List<UserView>();
            foreach (User user in result)
            {
                userViews.Add(new UserView()
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Username = user.UserName,
                });
            }
            return Ok(userViews);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(Loginmodel loginmodel)
        {
            var result = await _userRepon.Login(loginmodel.Username, loginmodel.Password);
            if (result == null)
            {
                return BadRequest();
            }
            /*UserView userView = new UserView() {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.UserName,
            };*/
            return Ok(result);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CreateAccUser createAccUser)
        {

            User user = await _userRepon.GetUserByUsername(createAccUser.UserName);
            if(user != null)
            {
                return BadRequest("trùng UserName");
            }
            user = new User()
            {
                Id = Guid.NewGuid(),
                UserName = createAccUser.UserName,
                Password = createAccUser.Password,
                FullName = createAccUser.FullName,
            };
            int result = await _userRepon.Create(user);

            if(result == 1) {
                return Ok("đã tạo thành công");
            }
            return BadRequest("Lỗi hệ thống");
        }
    }
}
