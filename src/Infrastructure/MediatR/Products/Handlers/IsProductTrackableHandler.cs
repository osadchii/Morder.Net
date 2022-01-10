using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Products.Queries;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Products.Handlers;

public class IsProductTrackableHandler : IRequestHandler<IsProductTrackableRequest, bool>
{
    private readonly MContext _context;

    public IsProductTrackableHandler(MContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(IsProductTrackableRequest request, CancellationToken cancellationToken)
    {
        var marketplaceData = await _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.Id == request.MarketplaceId)
            .Select(m => new { m.ProductTypes, m.Type })
            .SingleAsync(cancellationToken);

        var productTypes = marketplaceData.ProductTypes.FromJson<List<ProductType>>();

        bool validProductType = await _context.Products
            .AsNoTracking()
            .AnyAsync(p => !p.DeletionMark
                           && p.CategoryId.HasValue
                           && p.ProductType.HasValue
                           && productTypes!.Contains(p.ProductType.Value), cancellationToken);

        if (!validProductType)
        {
            return validProductType;
        }

        if (MarketplaceConstants.MarketplacesHasNoExternalProductId.Contains(marketplaceData.Type))
        {
            return true;
        }

        return await _context.MarketplaceProductSettings
            .AsNoTracking()
            .AnyAsync(
                s => s.ProductId == request.ProductId && s.MarketplaceId == request.MarketplaceId &&
                     s.ExternalId != null, cancellationToken);
    }
}