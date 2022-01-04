using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Commands;

public class SaveOrderStatusHistoryRequest : IRequest<Unit>
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
}