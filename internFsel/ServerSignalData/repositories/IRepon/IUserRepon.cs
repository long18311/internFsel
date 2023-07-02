

using ServerSignalData.Models;

namespace ServerSignalData.repositories.IRepon
{
    public interface IUserRepon
    {
        Task<string> Login(string username,string password);
        Task<List<User>> GetLstUser(Guid id);
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByUsername(string username);
        Task<int> Create(User user);
    }
}
