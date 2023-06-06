using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Products.Queries;
using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Products.Handlers;

public class IsProductTrackableHandler : IRequestHandler<IsProductTrackableRequest, bool>
{
    private readonly MContext _context;
    private readonly IProductIdentifierService _identifierService;

    public IsProductTrackableHandler(MContext context, IProductIdentifierService identifierService)
    {
        _context = context;
        _identifierService = identifierService;
    }

    public async Task<bool> Handle(IsProductTrackableRequest request, CancellationToken cancellationToken)
    {
        var marketplaceData = await _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.Id == request.MarketplaceId)
            .Select(m => new { m.ProductTypes, m.Type })
            .SingleAsync(cancellationToken);

        var productTypes = marketplaceData.ProductTypes.FromJson<List<ProductType>>();

        var validProductType = await _context.Products
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

        var identifier = await _identifierService.GetIdentifierAsync(request.MarketplaceId, request.ProductId,
            ProductIdentifierType.StockAndPrice);

        return identifier is not null;
    }
}