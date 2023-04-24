namespace OrderAPI.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public long TotalPrice { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
