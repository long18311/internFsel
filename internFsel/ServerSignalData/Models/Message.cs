namespace ServerSignalData.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content{ get; set; }
        public DateTime time { get; set; }
        public Guid UserSenderid { get; set; }
        public User UserSender { get; set; }
        public Guid UserReceiveid { get; set; }
        /*public User UserReceive { get; set; }*/
    }   
}
