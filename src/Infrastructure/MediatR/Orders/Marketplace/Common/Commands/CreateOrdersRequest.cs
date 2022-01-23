using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Marketplace.Common.Commands;

public class CreateOrdersRequest : IRequest<IEnumerable<Order>>
{
    public IEnumerable<CreateOrderRequest> CreateOrderRequests { get; set; }
}