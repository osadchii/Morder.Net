using System.Diagnostics;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.MediatR.Prices.Queries;
using Infrastructure.Models.Companies;
using Infrastructure.Models.Interfaces;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Prices.Handlers;

public class
    GetMarketplacePricesHandler : IRequestHandler<GetMarketplacePricesRequest, IEnumerable<MarketplacePriceDto>>
{
    private readonly MContext _context;
    private readonly ILogger<GetMarketplacePricesHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IProductIdentifierService _identifierService;
    
    public GetMarketplacePricesHandler(MContext context, IMediator mediator,
        ILogger<GetMarketplacePricesHandler> logger, IProductIdentifierService identifierService)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
        _identifierService = identifierService;
    }

    public async Task<IEnumerable<MarketplacePriceDto>> Handle(GetMarketplacePricesRequest request,
        CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var result = new List<MarketplacePriceDto>();

        List<Product> products = await _context.PriceChanges
            .AsNoTracking()
            .Where(s => s.MarketplaceId == request.MarketplaceId)
            .Include(s => s.Product)
            .OrderBy(p => p.ProductId)
            .Take(request.Limit)
            .Select(c => c.Product)
            .ToListAsync(cancellationToken);

        if (products.Count == 0)
        {
            return result;
        }

        IEnumerable<int> productIds = products.Select(p => p.Id).ToArray();

        Dictionary<int, string> externalIds = await _identifierService.GetIdentifiersAsync(request.MarketplaceId, productIds,
            ProductIdentifierType.StockAndPrice);

        CompanyDto companyInformation = await _mediator.Send(new GetCompanyInformationRequest(), cancellationToken);
        Marketplace marketplace = await _context.Marketplaces
            .AsNoTracking()
            .SingleAsync(m => m.Id == request.MarketplaceId, cancellationToken);

        Dictionary<int, decimal> basePrices = await _context.Prices
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

        result.AddRange(from product in products
            let externalId = marketplace.Type switch
            {
                MarketplaceType.Ozon => externalIds[product.Id],
                MarketplaceType.YandexMarket => externalIds[product.Id],
                _ => string.Empty
            }
            select new MarketplacePriceDto()
            {
                Articul = product.Articul!,
                MarketplaceId = marketplace.Id,
                ProductId = product.Id,
                Value = GetPrice(product),
                ProductExternalId = externalId ?? string.Empty
            });

        stopwatch.Stop();

        _logger.LogInformation("Handled price request for {MarketplaceName} with {Count} products elapsed {ElapsedMs} ms",
            marketplace.Name, products.Count, stopwatch.ElapsedMilliseconds);

        return result;

        decimal GetPrice(IHasId product)
        {
            if (specialPrices.TryGetValue(product.Id, out var value) && value > 0)
            {
                return value;
            }

            return basePrices.TryGetValue(product.Id, out value) ? value : 0;
        }
    }
}