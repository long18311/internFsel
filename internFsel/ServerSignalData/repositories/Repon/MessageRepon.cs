using Microsoft.EntityFrameworkCore;
using ServerSignalData.Confing;
using ServerSignalData.Models;
using ServerSignalData.repositories.IRepon;
using System.Linq;

namespace ServerSignalData.repositories.Repon
{
    public class MessageRepon : IMessageRepon
    {
        private readonly ManageAppDbContext _manageAppDb;
        public MessageRepon(ManageAppDbContext manageAppDb)
        {
            _manageAppDb = manageAppDb;
        }

        public async Task<int> Create(Message message)
        {
            try {
                await _manageAppDb.messages.AddAsync(message);
                await _manageAppDb.SaveChangesAsync();
                return 1;
            } catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<Message>> GetList(Guid UserSenderid, Guid UserReceiveid)
        {
            List<Message> messages = await _manageAppDb.messages.Where(a => (a.UserSenderid == UserSenderid&& a.UserReceiveid == UserReceiveid) || (a.UserSenderid == UserReceiveid && a.UserReceiveid == UserSenderid)).ToListAsync();
            if(messages==null)
            {
                return null;
            }
            return messages;

        }
    }
}
