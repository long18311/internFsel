using Microsoft.EntityFrameworkCore;
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
        public OrderRepon(DDBC dDBC)
        {
            this.dDBC = dDBC;
        }
        public async Task<Order> GetOrderByID(Guid Id)
        {
            try
            {
                var result = await dDBC.orders.FirstAsync(x => x.Id == Id); 
                return result;
            } catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Guid> GetCustomerIdBySdt(string PhoneNumber)
        {
            Customer customer = new Customer();
            using(var httpClient = new HttpClient()) { 
            customer = await(await httpClient.GetAsync($"https://localhost:7283/api/Customer/GetBySdt?sdt={PhoneNumber}"))
                .Content.ReadAsAsync<Customer>();
            }
            if (customer == null)
            {
                return new Guid();
            }
            return customer.Id;
        }
        
        public async Task<Guid> GetCustomerIdByNewCustomer(CreateCustomer createCustomer)
        {
            Customer customer = new Customer();
            using (var httpClient = new HttpClient())
            {
                 customer = await (await httpClient.PostAsJsonAsync($"https://localhost:7283/api/Customer/Createt", createCustomer))
                    .Content.ReadAsAsync<Customer>();
            }
            if(customer == null) {
                return new Guid();
            }
            return customer.Id;
        }
        public async Task<int> Create(CreateOrder model)
        {
            Guid customerId = await (GetCustomerIdBySdt(model.PhoneNumber));
            
            if (customerId == null || customerId == new Guid())
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
                
                
                customerId = await (GetCustomerIdByNewCustomer(createCustomer));
                if (customerId == null || customerId == new Guid())
                {
                    return 2;
                }
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
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                OrderDate = model.OrderDate,
                TotalPrice = tota,
                
                //OrderDetails = null
            };
            try
            {
                dDBC.orders.AddAsync(order);
            }
            catch (Exception ex)
            {
                return 4;
            }
            if (model.OrderDetails.Count() > 0 || model.OrderDetails != null)
            {
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
                try
                {
                    dDBC.orderDetails.AddRange(orderDetails);
                    await dDBC.SaveChangesAsync();
                }catch (Exception e)
                {
                    return 5;
                }

            }
            return 3;

        }
        public async Task<List<OrderDetail>> GetOrderHaveDetailbyId(Guid Id)
        {
            var lstOrderDetail = await dDBC.orderDetails.Where(x => x.OrderId == Id).ToListAsync();
            
            return lstOrderDetail;
        }
        public async Task<List<Order>> getAll()
        {
            var result = await dDBC.orders.Include(x => x.OrderDetails).ToListAsync();
            if (result == null)
            {
                return null;
            }
            

            return result;
        }
    }
}
