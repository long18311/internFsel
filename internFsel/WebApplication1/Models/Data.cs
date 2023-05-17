namespace WebApplication1.Models
{
    public class DataVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
    }
    public class Data : DataVM
    {
        public Guid Id { get; set; }
       
    }
}
