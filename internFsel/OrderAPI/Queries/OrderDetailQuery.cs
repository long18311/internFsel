using MediatR;
using OrderAPI.Models;

namespace OrderAPI.Queries
{
    public class OrderDetailQuery
    {
        public record GetOrderDetailAllListQuery() : IRequest<List<OrderDetail>>;
        public record GetOrderDetailByIdQuery(Guid id) : IRequest<OrderDetail>;
        public record GetOrderDetailByOrderIdListQuery(Guid orderid) : IRequest<List<OrderDetail>>;

    }
}
