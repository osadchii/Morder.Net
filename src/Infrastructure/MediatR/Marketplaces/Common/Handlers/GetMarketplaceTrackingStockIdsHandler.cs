using Infrastructure.MediatR.Marketplaces.Common.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Marketplaces.Common.Handlers;

public class GetMarketplaceTrackingStockIdsHandler : IRequestHandler<GetMarketplaceTrackingStockIdsRequest, List<int>>
{
    private readonly MContext _context;

    public GetMarketplaceTrackingStockIdsHandler(MContext context)
    {
        _context = context;
    }

    public Task<List<int>> Handle(GetMarketplaceTrackingStockIdsRequest request, CancellationToken cancellationToken)
    {
        return _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.IsActive && m.StockChangesTracking)
            .Select(m => m.Id)
            .ToListAsync(cancellationToken);
    }
}