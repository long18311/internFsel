using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerSignalData.Confing;
using ServerSignalData.Models;
using ServerSignalData.repositories.IRepon;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServerSignalData.repositories.Repon
{
    public class UserRepon : IUserRepon
    {
        private readonly ManageAppDbContext _manageAppDb;
        public UserRepon(ManageAppDbContext manageAppDb) {
            _manageAppDb = manageAppDb;
        }

        public async Task<int> Create(User user)
        {
            try {
                await _manageAppDb.users.AddAsync(user);
                await _manageAppDb.SaveChangesAsync();
                return 1;
            } catch(Exception e) { 
                return 0;
            }
        }

        public async Task<string> Login(string username, string password)
        {
            var user = await _manageAppDb.users.FirstOrDefaultAsync(p => p.UserName == username && p.Password == password);
            if (user == null)
            {
                return null;
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Id.ToString()),
                new Claim(ClaimTypes.UserData, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var authenkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisIsTheSecurityKey12345678"));
            var token = new JwtSecurityToken(
            issuer: "https://localhost:7194",
            audience: "InternFsel",
            expires: DateTime.Now.AddMinutes(120),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authenkey, SecurityAlgorithms.HmacSha512Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _manageAppDb.users.FirstOrDefaultAsync(p => p.UserName == username);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<User> GetUserById(Guid id)
        {
            var user = await _manageAppDb.users.FindAsync(id);
            return user;
        }

        public async Task<List<User>> GetLstUser(Guid id)
        {
            List<User> users = await _manageAppDb.users.Where(p => p.Id != id).ToListAsync();
            if(users == null) { return null; }
            return users;
        }
    }
}
