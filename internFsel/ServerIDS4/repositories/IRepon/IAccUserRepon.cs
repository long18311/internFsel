using Microsoft.AspNetCore.Identity;
using ServerIDS4.ViewModel;

namespace ServerIDS4.repositories.IRepon
{
    public interface IAccUserRepon
    {
        /*Task<List<IdentityUser>> GetList();*/
        Task<IdentityResult> AccUserCreate(CreateAccUser createAccUser);
    }
}
