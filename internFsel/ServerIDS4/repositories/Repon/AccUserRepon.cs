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
        private readonly UserManager<IdentityUser> _userManager;
        
        public AccUserRepon( UserManager<IdentityUser> userManager)
        {
            
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
            List<string> roles = new List<string>();
            if (createAccUser.view_customer)
            {
                roles.Add("view_customer");
            }
            if (createAccUser.create_customer)
            {
                roles.Add("create_customer");
            }
            if (createAccUser.update_customer)
            {
                roles.Add("update_customer");
            }
            if (createAccUser.delete_customer)
            {
                roles.Add("delete_customer");
            }
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
                    if (result.Succeeded && roles.Count>0)
                    {
                        result = _userManager.AddToRolesAsync(user, roles).Result;
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            
            
            return result;            
        }
    }
}
