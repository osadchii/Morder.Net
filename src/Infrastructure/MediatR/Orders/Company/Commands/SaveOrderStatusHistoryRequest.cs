using Infrastructure.Models.Orders;
using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Commands;

public class SaveOrderStatusHistoryRequest : IRequest<Unit>
{
    public int OrderId { get; init; }
    public OrderStatus Status { get; init; }
    public string User { get; set; } = null!;
}