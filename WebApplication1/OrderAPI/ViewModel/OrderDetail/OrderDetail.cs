namespace OrderAPI.ViewModel.OrderDetail
{
    public class OrderDetail
    {
        public string ProductName { get; set; }
        public long UnitPrice { get; set; }
        public int Quantity { get; set; }
        public long Total()
        {
            return UnitPrice * Quantity;
        }
    }
}
