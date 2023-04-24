using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        public DataController() { }
        public static List<Data> users = new List<Data>();
        [HttpGet]
        public IActionResult getAll()
        {
            return Ok(users);
        }
        [HttpGet("{id}")]
        public IActionResult getById(string id )
        {
            try {
                //LINQ [Object] QUERY
                var user = users.SingleOrDefault(hh => hh.Id == Guid.Parse(id));
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Create(DataVM userVM) { 
            var user = new Data() { 
                Id = Guid.NewGuid(),
                Fullname = userVM.Fullname,
                Password = userVM.Password,
                Username = userVM.Username
            };
            users.Add(user);
            return Ok(new
            {
                Success = true,
                Data = userVM
            });
        }
        [HttpPut("{id}")]
        public IActionResult Edit(string id, DataVM userVM) {
            try
            {
                var user = users.SingleOrDefault(hh => hh.Id == Guid.Parse(id));
                if (user == null)
                {
                    return NotFound();
                }
                //if (id != user.Id.ToString())
                //{
                //    return BadRequest();
                //}
                //Update
                user.Fullname = userVM.Fullname;
                user.Password = userVM.Password;
                user.Username = userVM.Username;
                return Ok(user);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Remove(string id)
        {

            try
            {
                var user = users.SingleOrDefault(hh => hh.Id == Guid.Parse(id));
                if (user == null)
                {
                    return NotFound();
                }
                //if (id != user.Id.ToString())
                //{
                //    return BadRequest();
                //}
                //Delete
                users.Remove(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    } 
}
