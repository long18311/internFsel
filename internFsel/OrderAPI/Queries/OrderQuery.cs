using MediatR;
using OrderAPI.Models;

namespace OrderAPI.Queries
{
    public class OrderQuery
    {
        public record GetOrderAllListQuery() : IRequest<List<Order>>;
        public record GetOrderListbyPhoneNumberQuery(string phoneNumber) : IRequest<List<Order>>;
        public record GetOrderByIdQuery(Guid id) : IRequest<Order>;
    }
}
