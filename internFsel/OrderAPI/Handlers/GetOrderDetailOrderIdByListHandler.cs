using MediatR;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using static OrderAPI.Queries.OrderDetailQuery;

namespace OrderAPI.Handlers
{
    public class GetOrderDetailOrderIdByListHandler : IRequestHandler<GetOrderDetailByOrderIdListQuery, List<OrderDetail>>
    {
        private readonly IOrderDetailRepon _orderDetailRepon;
        public GetOrderDetailOrderIdByListHandler(IOrderDetailRepon orderDetailRepon) {
            _orderDetailRepon = orderDetailRepon;
        }
        public async Task<List<OrderDetail>> Handle(GetOrderDetailByOrderIdListQuery request, CancellationToken cancellationToken)
        {
            return await _orderDetailRepon.GetListByOrderId(request.orderid);
        }
    }
}
