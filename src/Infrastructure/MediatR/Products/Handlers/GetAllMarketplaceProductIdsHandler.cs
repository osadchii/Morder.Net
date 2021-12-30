using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Products.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Products.Handlers;

public class GetAllMarketplaceProductIdsHandler : IRequestHandler<GetAllMarketplaceProductIdsRequest, List<int>>
{
    private readonly MContext _context;

    public GetAllMarketplaceProductIdsHandler(MContext context)
    {
        _context = context;
    }

    public async Task<List<int>> Handle(GetAllMarketplaceProductIdsRequest request, CancellationToken cancellationToken)
    {
        Marketplace marketplace = await _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.Id == request.MarketplaceId)
            .SingleAsync(cancellationToken);

        var productTypes = marketplace.ProductTypes.FromJson<List<ProductType>>();

        List<int> products = await _context.Products
            .AsNoTracking()
            .Where(p => !p.DeletionMark
                        && p.ProductType.HasValue
                        && productTypes!.Contains(p.ProductType!.Value))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);

        if (MarketplaceConstants.MarketplacesHasNoExternalProductId.Contains(marketplace.Type))
        {
            return products;
        }

        return await _context.MarketplaceProductSettings
            .AsNoTracking()
            .Where(s => products.Contains(s.ProductId) && s.MarketplaceId == request.MarketplaceId &&
                        s.ExternalId != null && s.ExternalId != string.Empty)
            .Select(s => s.ProductId)
            .ToListAsync(cancellationToken);
    }
}