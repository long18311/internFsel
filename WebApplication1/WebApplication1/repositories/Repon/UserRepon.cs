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
        public async Task<string> Login(Loginmodel loginmodel)
        {
            var user = await dDBC.users.FirstOrDefaultAsync(p => p.Username == loginmodel.Username && p.Password == loginmodel.Password);
            if (user == null) return null;
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginmodel.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var authenkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisIsTheSecurityKey12345678"));
            var token = new JwtSecurityToken(
            issuer: "https://localhost:7283",
            audience: "InternFsel",
            expires: DateTime.Now.AddMinutes(30),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authenkey,SecurityAlgorithms.HmacSha512Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
