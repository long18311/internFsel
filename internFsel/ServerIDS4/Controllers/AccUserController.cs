using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerIDS4.repositories.IRepon;
using ServerIDS4.ViewModel;

namespace ServerIDS4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AccUserController : ControllerBase
    {
        private readonly IAccUserRepon _accUserRepon;
        public AccUserController(IAccUserRepon accUserRepon)
        {
            _accUserRepon = accUserRepon;
        }
        /*{
  "userName": "nhan",
  "lastName": "nguyen",
  "email": "longnguyen@example.com",
  "password": "Pass@123",
  "view_customer": true,
  "create_customer": true,
  "update_customer": false,
  "delete_customer": true
}*/

        [HttpPost]
        /*[Authorize]*/
        public async Task<IActionResult> CreateAccUser([FromBody] CreateAccUser createAccUser) {
            Console.WriteLine("vui");
           var result = await _accUserRepon.AccUserCreate(createAccUser);
            if(result.Succeeded) {
                return Ok(1);
            }
            else { return Ok(result); }
           return Ok("vyi");
            
        }
    }
}
