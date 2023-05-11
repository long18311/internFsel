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

        public async Task<int> Create(OrderDetail orderDetail)
        {
            try
            {
                dDBC.orderDetails.AddAsync(orderDetail);
                dDBC.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<int> Update(OrderDetail orderDetail)
        {
            
            try {
                dDBC.orderDetails.UpdateRange(orderDetail);
                
                dDBC.SaveChanges();
                return 1;
            } catch(Exception ex) { return 0; }

        }
        public async Task<int> Delete(OrderDetail orderDetail)
        {
            
            try {
                dDBC.orderDetails.Remove(orderDetail);                
                dDBC.SaveChanges();
                return 1;
            }
            catch(Exception ex) { return 0; }
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

        

        public async Task<List<OrderDetail>> GetListByOrderId(Guid orderid)
        {
            var lstOrderDetail = await dDBC.orderDetails.Where(x => x.OrderId == orderid).ToListAsync();
            if (lstOrderDetail == null) return null;
            return lstOrderDetail;
        }
    }
}
