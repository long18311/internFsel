using WebApplication1.Models;
using WebApplication1.ViewModel.User;

namespace WebApplication1.repositories.IRepon
{
    public interface IUserRepon
    {
        Task<User> GetUserbyLoginmodel(Loginmodel loginmodel);
    }
}
