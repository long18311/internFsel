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
        //private readonly HttpClient httpClient = new HttpClient();
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
            } catch (Exception ex) {
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
        public async Task<int> Create(Order order, List<OrderDetail> orderDetails)
        {

            try
            {
                dDBC.orders.AddAsync(order);
                dDBC.orderDetails.AddRange(orderDetails);
                await dDBC.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<List<OrderDetail>> GetOrderDetailbyId(Guid orderId)
        {
            var lstOrderDetail = await dDBC.orderDetails.Where(x => x.OrderId == orderId).ToListAsync();
            foreach (var orderDetail in lstOrderDetail)
            {
                orderDetail.Order = null;
            }

            return lstOrderDetail;
        }

        public async Task<List<Order>> getAll()
        {
            var result = await dDBC.orders.ToListAsync();//.Include(x => x.OrderDetails)

            return result;
        }

        public async Task<int> Update(Order order)
        {
            try {
                dDBC.orders.UpdateRange(order);
                dDBC.SaveChangesAsync();
                return 1;

            } catch (Exception e) { return 0; }


        }

        public async Task<int> Delete(Order order)
        {

            try
            {
                List<OrderDetail> orderDetails = await GetOrderDetailbyId(order.Id);
                dDBC.orderDetails.RemoveRange(orderDetails);
                dDBC.orders.Remove(order);
                dDBC.SaveChangesAsync();
                return 1;
            }
            catch (Exception e) { return 0; }
        }
        public async Task<List<Order>> getlistbyCustomerid(Guid id)
        {
            var lstOrder = await dDBC.orders.Where(x => x.CustomerId == id).ToListAsync();
            return lstOrder;
        }
        public async Task<Order> GetOrderById(Guid id)
        {
            var order = await dDBC.orders.FindAsync(id);
            return order;
        }
        }
    
}
