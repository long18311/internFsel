 using WebApplication1.ViewModel.User;

namespace WebApplication1.repositories.IRepon
{
    public interface IUserRepon
    {
        Task<string> Login(Loginmodel loginmodel);
    }
}
