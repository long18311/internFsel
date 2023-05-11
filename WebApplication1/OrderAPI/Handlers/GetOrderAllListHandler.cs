using MediatR;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using static OrderAPI.Queries.OrderQuery;

namespace OrderAPI.Handlers
{
    public class GetOrderAllListHandler : IRequestHandler<GetOrderAllListQuery, List<Order>>
    {
        private readonly IOrderRepon _orderRepon;
        public GetOrderAllListHandler(IOrderRepon orderRepon) {
            _orderRepon = orderRepon;
        }
        public async Task<List<Order>> Handle(GetOrderAllListQuery request, CancellationToken cancellationToken)
        {
            var result = await _orderRepon.getAll();
            foreach (var order in result)
            {
                order.OrderDetails = await _orderRepon.GetOrderDetailbyId(order.Id);
            }
            if (result == null)
            {
                return null;
            }
            return result;
        }
    }
}
