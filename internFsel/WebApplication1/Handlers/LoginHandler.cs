using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.repositories.IRepon;
using WebApplication1.ViewModel.User;
using static WebApplication1.Queries.UserQuery;

namespace WebApplication1.Handlers
{
    public class LoginHandler : IRequestHandler<LoginQuery, string>
    {
        IUserRepon _userRepon;
        public LoginHandler(IUserRepon userRepon) {
            _userRepon = userRepon;
        }
        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = _userRepon.GetUserbyLoginmodel(request.loginmodel);
            if (user == null) return null;
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, request.loginmodel.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var authenkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisIsTheSecurityKey12345678"));
            var token = new JwtSecurityToken(
            issuer: "https://localhost:7283",
            audience: "InternFsel",
            expires: DateTime.Now.AddMinutes(30),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authenkey, SecurityAlgorithms.HmacSha512Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
