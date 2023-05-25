using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ServerIDS4.Data;
using ServerIDS4.repositories.IRepon;
using ServerIDS4.ViewModel;
using System.Security.Claims;

namespace ServerIDS4.repositories.Repon
{
    public  class AccUserRepon : IAccUserRepon
    {
        private readonly AspNetIdentityDbContext _aspNetIdentityDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public AccUserRepon(AspNetIdentityDbContext context, UserManager<IdentityUser> userManager)
        {
            _aspNetIdentityDbContext = context;
            _userManager = userManager;
        }
        public async Task<IdentityResult> AccUserCreate(CreateAccUser createAccUser)
        {
            //using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            IdentityUser user = new IdentityUser
            {
                Email = createAccUser.Email,
                UserName = createAccUser.UserName
            };
            IdentityResult result = null;
            try {
                result = await _userManager.CreateAsync(user, createAccUser.Password);
                if (result.Succeeded)
                {
                    result = _userManager.AddClaimsAsync(user, new Claim[]
                            {
                            new Claim(JwtClaimTypes.Name, createAccUser.UserName +" "+ createAccUser.LastName),
                            new Claim(JwtClaimTypes.GivenName, createAccUser.UserName),
                            new Claim(JwtClaimTypes.FamilyName, createAccUser.LastName),
                            new Claim(JwtClaimTypes.WebSite, "http://"+createAccUser.UserName + createAccUser.LastName+".com"),
                            new Claim("location", "somewhere")
                            }
                        ).Result;
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            
            
            return result;            
        }
    }
}
