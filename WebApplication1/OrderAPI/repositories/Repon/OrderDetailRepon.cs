using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.ViewModel.OrderDetail;

namespace OrderAPI.repositories.Repon
{
    public class OrderDetailRepon : IOrderDetailRepon
    {
        private readonly DDBC dDBC;
        private readonly IOrderRepon orderRepon;
        public OrderDetailRepon(DDBC dDBC,IOrderRepon orderRepon)
        {
            this.dDBC = dDBC;
            this.orderRepon = orderRepon;
        }

        public async Task<int> Create(CreateOrderDetail createOrderDetail)
        {
            Order order = await orderRepon.GetOrderById(createOrderDetail.OrderId);
            if(order == null)
            {
                return 1;
            }
            order.TotalPrice = order.TotalPrice + (createOrderDetail.Quantity * createOrderDetail.UnitPrice);
            OrderDetail orderDetail = new OrderDetail() {
                Id = Guid.NewGuid(),
                ProductName = createOrderDetail.ProductName,
                Quantity = createOrderDetail.Quantity,
                UnitPrice = createOrderDetail.UnitPrice,
                Order = order,
                OrderId = order.Id,
            };
            try {
                dDBC.orderDetails.AddAsync(orderDetail);
                dDBC.orders.UpdateRange(order);
                dDBC.SaveChanges();
                return 2;
            }catch(Exception ex) {
                return 3;
            }
        }
        public async Task<int> Update(UpdateOrderDetail updateOrderDetail)
        {
            OrderDetail orderDetail = await dDBC.orderDetails.FindAsync(updateOrderDetail.Id);
            if (orderDetail == null)
            {
                return 1;
            }
            Order order = await orderRepon.GetOrderById(orderDetail.OrderId);
            order.TotalPrice = order.TotalPrice + (orderDetail.Quantity * orderDetail.UnitPrice - updateOrderDetail.Quantity* updateOrderDetail.UnitPrice);
            orderDetail.ProductName = updateOrderDetail.ProductName;
            orderDetail.Quantity = updateOrderDetail.Quantity;
            orderDetail.UnitPrice = updateOrderDetail.UnitPrice;
            try {
                dDBC.orderDetails.UpdateRange(orderDetail);
                dDBC.orders.UpdateRange(order); 
                dDBC.SaveChanges();
                return 2;
            } catch(Exception ex) { return 3; }

        }
        public async Task<int> Delete(Guid id)
        {
            OrderDetail orderDetail = await dDBC.orderDetails.FindAsync(id);
            if(orderDetail == null)
            {
                return 1;
            }
            Order order = await orderRepon.GetOrderById(orderDetail.OrderId);
            order.TotalPrice = order.TotalPrice - (orderDetail.Quantity * orderDetail.UnitPrice);
            try {
                dDBC.orderDetails.Remove(orderDetail);
                dDBC.orders.UpdateRange(order);
                dDBC.SaveChanges();
                return 3;
            }
            catch(Exception ex) { return 4; }
        }

        public async Task<List<OrderDetail>> GetAll()
        {
            var lstOrderDetail = await dDBC.orderDetails.ToListAsync();
            return lstOrderDetail;
        }

        public async Task<OrderDetail> GetOrderDetailById(Guid id)
        {
            var orderDetail = await dDBC.orderDetails.FindAsync(id);
            if(orderDetail == null) return null;
            return orderDetail;
        }

        

        public async Task<List<OrderDetail>> Getlist(Guid Orderid)
        {
            var lstOrderDetail = await dDBC.orderDetails.Where(x => x.OrderId == Orderid).ToListAsync();
            if (lstOrderDetail == null) return null;
            return lstOrderDetail;
        }
    }
}
