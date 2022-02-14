using Infrastructure.Common;
using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Commands;

public class GetChangedOrdersRequest : IRequest<Result>
{
    public bool ClearRegistration { get; set; }
}