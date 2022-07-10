using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Products.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Products.Handlers;

public class GetAllMarketplaceProductIdsHandler : IRequestHandler<GetAllMarketplaceProductIdsRequest, List<int>>
{
    private readonly MContext _context;
    private readonly IProductIdentifierService _identifierService;

    public GetAllMarketplaceProductIdsHandler(MContext context, IProductIdentifierService identifierService)
    {
        _context = context;
        _identifierService = identifierService;
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

        Dictionary<int, string?> externalIds =
            await _identifierService.GetIdentifiersAsync(request.MarketplaceId, products,
                ProductIdentifierType.StockAndPrice);

        return externalIds.Keys.ToList();
    }
}