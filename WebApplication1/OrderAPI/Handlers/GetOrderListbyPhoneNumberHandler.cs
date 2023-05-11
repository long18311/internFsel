using MediatR;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using static OrderAPI.Queries.OrderQuery;

namespace OrderAPI.Handlers
{
    public class GetOrderListbyPhoneNumberHandler : IRequestHandler<GetOrderListbyPhoneNumberQuery, List<Order>>
    {
        private readonly IOrderRepon _orderRepon;
        public GetOrderListbyPhoneNumberHandler(IOrderRepon orderRepon)
        {
            _orderRepon = orderRepon;
        }
        public async Task<List<Order>> Handle(GetOrderListbyPhoneNumberQuery request, CancellationToken cancellationToken)
        {
            Guid customerid = await _orderRepon.GetCustomerIdBySdt(request.phoneNumber);
            if (customerid == null || customerid == new Guid())
            {
                return null;
            }
            var lstOrder = await _orderRepon.getlistbyCustomerid(customerid);
            foreach (var order in lstOrder)
            {
                order.OrderDetails = await _orderRepon.GetOrderDetailbyId(order.Id);
            }
            return lstOrder;
        }
    }
}
