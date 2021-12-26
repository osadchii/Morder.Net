using System.Diagnostics;
using Infrastructure.Marketplaces;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.MediatR.Prices.Queries;
using Infrastructure.Models.Companies;
using Infrastructure.Models.MarketplaceProductSettings;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
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

    public GetMarketplacePricesHandler(MContext context, IMediator mediator,
        ILogger<GetMarketplacePricesHandler> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
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

        IEnumerable<int> productIds = products.Select(p => p.Id);

        Dictionary<int, MarketplaceProductSetting> productSettings = await _context.MarketplaceProductSettings
            .AsNoTracking()
            .Where(ps => ps.MarketplaceId == request.MarketplaceId && productIds.Contains(ps.ProductId))
            .ToDictionaryAsync(ps => ps.ProductId, ps => ps, cancellationToken);

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

        foreach (Product product in products)
        {
            productSettings.TryGetValue(product.Id, out MarketplaceProductSetting? productSetting);

            string? externalId = marketplace.Type switch
            {
                MarketplaceType.Ozon => productSetting?.ExternalId,
                MarketplaceType.YandexMarket => productSetting?.ExternalId,
                _ => product.Articul
            };
            result.Add(new MarketplacePriceDto()
            {
                MarketplaceId = marketplace.Id,
                ProductId = product.Id,
                Value = GetPrice(product),
                ProductExternalId = externalId ?? string.Empty
            });
        }

        stopwatch.Stop();

        _logger.LogInformation($"Handled price request for " +
                               $"{request.MarketplaceId} with " +
                               $"{products.Count} elapsed " +
                               $"{stopwatch.ElapsedMilliseconds} ms");

        return result;

        decimal GetPrice(Product product)
        {
            if (specialPrices.TryGetValue(product.Id, out decimal value) && value > 0)
            {
                return value;
            }

            if (basePrices.TryGetValue(product.Id, out value))
            {
                return value;
            }

            return 0;
        }
    }
}