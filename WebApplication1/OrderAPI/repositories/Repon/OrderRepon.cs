using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.Service;
using OrderAPI.ViewModel.Order;
using System.Net.Http.Json;
using System.Text;

namespace OrderAPI.repositories.Repon
{
    public class OrderRepon : IOrderRepon
    {
        private readonly DDBC dDBC;
        private readonly HttpClient httpClient = new HttpClient();
        private readonly IApiCustomerService customerService;
        public OrderRepon(DDBC dDBC, IApiCustomerService customerService)
        {
            this.dDBC = dDBC;
            this.customerService = customerService;
        }
        public async Task<Order> GetOrderByID(Guid Id)
        {
            try
            {
                var result = await dDBC.orders.FindAsync(Id); 
                return result;
            } catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Guid> GetCustomerIdBySdt(string PhoneNumber)
        {
            Customer customer;
            try {
               customer = await customerService.GetBySdt(PhoneNumber);
            }catch (Exception ex) {
                return new Guid();
            }
           
            
            //customer = await(await httpClient.GetAsync($"https://localhost:7186/gateway/Customer/GetBySdt?sdt={PhoneNumber}"))
            //    .Content.ReadAsAsync<Customer>();
            
            if (customer == null)
            {
                return new Guid();
            }
            return customer.Id;
        }

        public async Task<Guid> GetCustomerIdByNewCustomer(CreateCustomer createCustomer)
        {

            try
            {
                Customer customer = await customerService.Createt(createCustomer);
                if (customer == null)
                {
                    return new Guid();
                }
                return customer.Id;

            }
            catch (Exception ex)
            {
                return new Guid();
            }
            //customer = await (await httpClient.PostAsJsonAsync($"https://localhost:7186/gateway/Customer/Createt", createCustomer))
            //  .Content.ReadAsAsync<Customer>();
        }  
        public async Task<int> Create(CreateOrder model)
        {

            Guid customerId = await (GetCustomerIdBySdt(model.PhoneNumber));
            
            if (customerId == null || customerId == new Guid())
            {
                if (model.Birthday == new DateTime() || model.Birthday == null || model.Fullname == null || model.Address == null || model.Email == null || model.Fullname == "string" || model.Address == "string" || model.Email == "string")
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
        public async Task<List<OrderDetail>> GetOrderDetailbyId(Guid Id)
        {
            var lstOrderDetail = await dDBC.orderDetails.Where(x => x.OrderId == Id).ToListAsync();
            foreach (var orderDetail in lstOrderDetail)
            {
                orderDetail.Order = null;
            }

            return lstOrderDetail;
        }

        public async Task<List<Order>> getAll()
        {
            var result = await dDBC.orders.ToListAsync();//.Include(x => x.OrderDetails)
            foreach (var order in result)
            {
                order.OrderDetails = await GetOrderDetailbyId(order.Id);
            }
            if (result == null)
            {
                return null;
            }
            

            return result;
        }

        public async Task<int> Update(UpdateOrder model)
        {
            if (model == null)
            {
                return 1;
            }
            Guid customerid = await GetCustomerIdBySdt(model.PhoneNumber);
            if (customerid == null || customerid == new Guid())
            {
                return 2;
            }
            var order = await dDBC.orders.FindAsync(model.Id);
            if (order == null)
            {
                return 3;
            }
            order.CustomerId = customerid;
            order.OrderDate = model.OrderDate;
            try {
                dDBC.orders.UpdateRange(order);
                dDBC.SaveChangesAsync();
                return 4;
            
            }catch (Exception e) { return 5; }
           return 5;
            
        }

        public async Task<int> Delete(Guid id)
        {
            Order order = await dDBC.orders.FindAsync(id);
            if (order == null)
            {
                return 1;
            }
            try {
                List<OrderDetail> orderDetails = await GetOrderDetailbyId(order.Id);
                dDBC.orderDetails.RemoveRange(orderDetails);
                dDBC.orders.Remove(order);
                dDBC.SaveChangesAsync();
                return 2;
            }catch (Exception e) { return 3 ; }
        }
        
        public async Task<List<Order>> getlst(string phoneNumber)
        {
            Guid customerid = await GetCustomerIdBySdt(phoneNumber);
            if(customerid == null || customerid == new Guid())
            {
                return null;
            }
            var lstOrder = await dDBC.orders.Where(x => x.CustomerId == customerid).ToListAsync();
            foreach (var order in lstOrder)
            {
                order.OrderDetails = await GetOrderDetailbyId(order.Id);
            }
            return lstOrder;
        }

        public async Task<Order> GetOrderById(Guid id)
        {
            var order = await dDBC.orders.FindAsync(id);
            List<OrderDetail> orderDetails = await GetOrderDetailbyId(order.Id);
            order.OrderDetails = orderDetails;
            return order;
        }
    }
}
