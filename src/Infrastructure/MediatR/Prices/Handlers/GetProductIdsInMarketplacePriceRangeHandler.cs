using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.MediatR.Prices.Queries;
using Infrastructure.MediatR.Products.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Prices.Handlers;

public class
    GetProductIdsInMarketplacePriceRangeHandler : IRequestHandler<GetProductIdsInMarketplacePriceRangeRequest,
        IEnumerable<int>>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public GetProductIdsInMarketplacePriceRangeHandler(MContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<IEnumerable<int>> Handle(GetProductIdsInMarketplacePriceRangeRequest request,
        CancellationToken cancellationToken)
    {
        var companyInformation = await _mediator.Send(new GetCompanyInformationRequest(), cancellationToken);
        var marketplace = await _context.Marketplaces
            .AsNoTracking()
            .SingleAsync(m => m.Id == request.MarketplaceId, cancellationToken);

        var productIds = await _mediator.Send(new GetAllMarketplaceProductIdsRequest
            { MarketplaceId = request.MarketplaceId });

        var basePrices = await _context.Prices
            .AsNoTracking()
            .Where(p => p.PriceTypeId == companyInformation.PriceTypeId && productIds.Contains(p.ProductId))
            .ToDictionaryAsync(p => p.ProductId, p => p.Value, cancellationToken);

        Dictionary<int, decimal> specialPrices = new();

        if (marketplace.PriceTypeId.HasValue && marketplace.PriceTypeId.Value != companyInformation.PriceTypeId)
        {
            specialPrices = await _context.Prices
                .AsNoTracking()
                .Where(p => p.PriceTypeId == marketplace.PriceTypeId.Value && productIds.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId, p => p.Value, cancellationToken);
        }

        return productIds
            .Select(productId => new { productId, price = GetPrice(productId) })
            .Where(p => p.price >= request.MinimalPrice && p.price <= request.MaximalPrice)
            .Select(p => p.productId);

        decimal GetPrice(int productId)
        {
            if (specialPrices.TryGetValue(productId, out var value) && value > 0)
            {
                return value;
            }

            if (basePrices.TryGetValue(productId, out value))
            {
                return value;
            }

            return 0;
        }
    }
}