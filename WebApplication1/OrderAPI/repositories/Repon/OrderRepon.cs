using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.ViewModel.Order;
using System.Net.Http.Json;
using System.Text;

namespace OrderAPI.repositories.Repon
{
    public class OrderRepon : IOrderRepon
    {
        private readonly DDBC dDBC;
        private readonly HttpClient httpClient = new HttpClient();
        public OrderRepon() { }
        public OrderRepon(DDBC dDBC)
        {
            this.dDBC = dDBC;
        }
        public async Task<int> Create(CreateOrder model)
        {
            Guid customerId;
            Customer customer = new Customer();
            customer = await (await httpClient.GetAsync($"https://localhost:7283/api/Customer/GetBySdt?sdt={model.PhoneNumber}"))
                .Content.ReadAsAsync<Customer>();

            customerId = customer.Id;
            if (customer == null)
            {
                if (model.Birthday == new DateTime() || model.Birthday == null || model.Fullname == null || model.Address == null || model.Email == null)
                {
                    return 1;
                }
                var createCustomer = new CreateCustomer()
                {
                    
                    Fullname = model.Fullname,
                    Address = model.Address,
                    Email = model.Email,
                    Birthday = (DateTime)model.Birthday,
                    PhoneNumber = model.PhoneNumber,
                };
                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(createCustomer), Encoding.UTF8, "application/json");
                Customer index = await (await httpClient.PostAsync($"https://localhost:7283/api/Customer/Createt", content))
                    .Content.ReadAsAsync<Customer>();
                if (index != null)
                {
                    return 2;
                }
                customerId = index.Id;
            }
            long tota = 0;
            if (model.OrderDetails.Count() > 0 || model.OrderDetails != null)
            {
                foreach (OrderDetail_CreateOrder item in model.OrderDetails)
                {
                    tota = tota + item.Total();
                }
            }
            //decimal
            Order order = new Order()
            {
                Id =   Guid.NewGuid(),
                CustomerId = customerId,
                OrderDate = model.OrderDate,
                TotalPrice = tota,
                
                //OrderDetails = null
            };
            try
            {
                dDBC.orders.AddAsync(order);
                dDBC.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 4;
            }
            if (model.OrderDetails.Count() > 0 || model.OrderDetails != null)
            {
                order = await dDBC.orders.FindAsync(order.Id);
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (OrderDetail_CreateOrder item in model.OrderDetails)
                {
                    orderDetails.Add(new OrderDetail()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        Order = order,
                        ProductName = item.ProductName,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity
                    });
                }
                dDBC.orderDetails.AddRange(orderDetails);
                dDBC.SaveChangesAsync();

            }
            return 3;

        }
    }
}
