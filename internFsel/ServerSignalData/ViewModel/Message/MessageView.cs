using Microsoft.AspNetCore.Mvc.RazorPages;
using ServerSignalData.ViewModel.User;

namespace ServerSignalData.ViewModel.Message
{
    public class MessageView
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime time { get; set; }
        public Guid UserSenderid { get; set; }
        public Guid UserReceiveid { get; set; }
    }
}
