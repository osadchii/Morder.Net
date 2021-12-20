using Infrastructure.MediatR.Marketplaces.Common.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Marketplaces.Common.Handlers;

public class GetMarketplaceTrackingPriceIdsHandler : IRequestHandler<GetMarketplaceTrackingPriceIdsRequest, List<int>>
{
    private readonly MContext _context;

    public GetMarketplaceTrackingPriceIdsHandler(MContext context)
    {
        _context = context;
    }

    public Task<List<int>> Handle(GetMarketplaceTrackingPriceIdsRequest request, CancellationToken cancellationToken)
    {
        return _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.IsActive && m.PriceChangesTracking)
            .Select(m => m.Id)
            .ToListAsync(cancellationToken);
    }
}