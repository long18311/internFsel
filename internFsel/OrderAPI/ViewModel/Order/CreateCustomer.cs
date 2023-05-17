

using System.ComponentModel.DataAnnotations;

namespace OrderAPI.ViewModel.Order
{
    public class CreateCustomer
    {
        public string Fullname { get; set; }        
        public DateTime Birthday { get; set; }        
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
