using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Commands;

public class SaveOrdersStatusHistoryRequest : IRequest<Unit>
{
    public IEnumerable<SaveOrderStatusHistoryRequest> Requests { get; set; }
}