using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;
using WebApplication1.repositories.IRepon;
using WebApplication1.ViewModel.User;

namespace WebApplication1.repositories.Repon
{
    public class UserRepon : IUserRepon
    {
        private readonly DDBC dDBC;
        public UserRepon(DDBC dDBC) {
            this.dDBC = dDBC;
        }
        public async Task<User> GetUserbyLoginmodel(Loginmodel loginmodel)
        {
            var user = await dDBC.users.FirstOrDefaultAsync(p => p.Username == loginmodel.Username && p.Password == loginmodel.Password);
            
            return user;
        }
    }
}
