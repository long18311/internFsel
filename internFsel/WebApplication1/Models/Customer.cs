namespace WebApplication1.Models
{
    public class Customer
    {
        public Guid Id { get; set; }  
        public string Fullname { get; set; }
        public DateTime Birthday{ get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
