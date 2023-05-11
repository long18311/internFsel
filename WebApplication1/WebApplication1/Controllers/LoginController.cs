using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.repositories.IRepon;
using WebApplication1.repositories.Repon;
using WebApplication1.ViewModel.Customer;
using WebApplication1.ViewModel.User;
using static WebApplication1.Queries.UserQuery;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoginController(IMediator mediator) {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Login(Loginmodel userVM)
        {
            string token = await _mediator.Send(new LoginQuery(userVM));
            if (token == null)
            {
                return BadRequest("Sai tài khoản mật khẩu");
            }
            return Ok(token);
        }
    }
}
