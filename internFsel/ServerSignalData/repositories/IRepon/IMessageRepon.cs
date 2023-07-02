using ServerSignalData.Models;


namespace ServerSignalData.repositories.IRepon
{
    public interface IMessageRepon
    {
        Task<List<Message>> GetList(Guid UserSenderid,Guid UserReceiveid);
        Task<int> Create(Message message);
    }
}
