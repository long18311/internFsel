namespace Client.Data
{
    public class OrderDetail
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public long UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}