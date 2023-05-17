using MediatR;
using OrderAPI.Models;
using OrderAPI.repositories.IRepon;
using OrderAPI.ViewModel.Order;
using static OrderAPI.Commands.OrderCommand;

namespace OrderAPI.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IOrderRepon _orderRepon;
        public CreateOrderHandler(IOrderRepon orderRepon)
        {
            _orderRepon = orderRepon;
        }
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            
            Guid customerId = await (_orderRepon.GetCustomerIdBySdt(request.model.PhoneNumber));

            if (customerId == null || customerId == new Guid())
            {
                if (request.model.Birthday == new DateTime() || request.model.Birthday == null || request.model.Fullname == null || request.model.Address == null || request.model.Email == null || request.model.Fullname == "string" || request.model.Address == "string" || request.model.Email == "string")
                {
                    return 2;
                }
                var createCustomer = new CreateCustomer()
                {

                    Fullname = request.model.Fullname,
                    Address = request.model.Address,
                    Email = request.model.Email,
                    Birthday = (DateTime)request.model.Birthday,
                    PhoneNumber = request.model.PhoneNumber,
                };


                customerId = await(_orderRepon.GetCustomerIdByNewCustomer(createCustomer));
                if (customerId == null || customerId == new Guid())
                {
                    return 3;
                }
            }
            long tota = 0;
            if (request.model.OrderDetails.Count() > 0 || request.model.OrderDetails != null)
            {
                foreach (OrderDetail_CreateOrder item in request.model.OrderDetails)
                {
                    tota = tota + item.Total();
                }
            }
            //decimal
            Order order = new Order()
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                OrderDate = request.model.OrderDate,
                TotalPrice = tota,

                //OrderDetails = null
            };
            List<OrderDetail> orderDetails;
            if (request.model.OrderDetails.Count() > 0 || request.model.OrderDetails != null)
            {
                 orderDetails = new List<OrderDetail>();
                foreach (OrderDetail_CreateOrder item in request.model.OrderDetails)
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
                return await _orderRepon.Create(order, orderDetails);

            }
            return 4;

        }
    }
}
