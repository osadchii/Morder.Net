using System.Net;
using Infrastructure.Common;
using Infrastructure.MediatR.Orders.Company.Queries;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Orders.Company.Handlers;

public class GetOrderByExternalIdHandler : IRequestHandler<GetOrderByExternalIdRequest, Result>
{
    private readonly MContext _context;

    public GetOrderByExternalIdHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(GetOrderByExternalIdRequest request, CancellationToken cancellationToken)
    {
        Order order = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .ThenInclude(p => p.Product)
            .Include(o => o.Boxes)
            .ThenInclude(b => b.Product)
            .SingleOrDefaultAsync(o => o.ExternalId == request.ExternalId!.Value, cancellationToken);

        if (order is null)
        {
            throw new HttpRequestException("Order not found", null, HttpStatusCode.NotFound);
        }

        return order.AsResult();
    }
}