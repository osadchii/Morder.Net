using Infrastructure.MediatR.Marketplaces.Common.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Marketplaces.Common.Handlers;

public class GetMarketplaceWarehouseIdHandler : IRequestHandler<GetMarketplaceWarehouseIdRequest, int>
{
    private readonly MContext _context;

    public GetMarketplaceWarehouseIdHandler(MContext context)
    {
        _context = context;
    }

    public Task<int> Handle(GetMarketplaceWarehouseIdRequest request, CancellationToken cancellationToken)
    {
        return _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.Id == request.MarketplaceId)
            .Select(m => m.WarehouseId)
            .SingleAsync(cancellationToken);
    }
}