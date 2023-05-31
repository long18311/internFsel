using Microsoft.AspNetCore.Identity;
using ServerIDS.ViewModel;

namespace ServerIDS.repositories.IRepon
{
    public interface IAccUserRepon
    {
        /*Task<List<IdentityUser>> GetList();*/
        Task<IdentityResult> AccUserCreate(CreateAccUser createAccUser);
    }
}
