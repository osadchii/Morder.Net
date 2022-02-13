using Infrastructure.MediatR.Products.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Products.Handlers;

public class
    GetExternalIdsByProductIdsHandler : IRequestHandler<GetExternalIdsByProductIdsRequest, Dictionary<int, string>>
{
    private readonly MContext _context;

    public GetExternalIdsByProductIdsHandler(MContext context)
    {
        _context = context;
    }

    public Task<Dictionary<int, string>> Handle(GetExternalIdsByProductIdsRequest request,
        CancellationToken cancellationToken)
    {
        return _context.MarketplaceProductSettings
            .AsNoTracking()
            .Where(s => s.MarketplaceId == request.MarketplaceId
                        && request.ProductIds.Contains(s.ProductId))
            .Select(s => new { s.ProductId, s.ExternalId })
            .ToDictionaryAsync(s => s.ProductId, s => s.ExternalId!, cancellationToken);
    }
}