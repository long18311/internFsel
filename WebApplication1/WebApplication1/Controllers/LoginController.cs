using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.repositories.IRepon;
using WebApplication1.repositories.Repon;
using WebApplication1.ViewModel.User;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepon _userRepon;
        public LoginController(IUserRepon userRepon) { 
            _userRepon = userRepon;
        }
        [HttpPost]
        public async Task<IActionResult> Login(Loginmodel userVM)
        {
            string token = await _userRepon.Login(userVM);
            if (token == null)
            {
                return BadRequest("Sai tài khoản mật khẩu");
            }
            return Ok(token);
        }
    }
}
