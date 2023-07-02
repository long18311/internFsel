using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class HomeController : ControllerBase
    {
        private readonly IHubContext<SignalHub> _hubContext;

        public HomeController(IHubContext<SignalHub> hubContext)
        {
            _hubContext = hubContext;
        }
        [HttpGet]
        [Route("/PushEmployee")]
        public async Task<IActionResult> PushEmployee(int id, string name, string Address) { 
         
            Employee employee = new Employee();
            employee.Id = id;
            employee.Name = name;
            employee.Address = Address;
            await _hubContext.Clients.All.SendAsync("ReceiveEmployee", employee);
            return Ok("Done");

        }
        [HttpGet]
        [Route("/PushMessage")]
        public async Task<IActionResult> Message(string user,string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);
            return Ok("Done");

        }
    }
}
