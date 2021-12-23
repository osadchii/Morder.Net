using System.Diagnostics;
using Infrastructure.Marketplaces;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.MediatR.Stocks.Queries;
using Infrastructure.Models.Companies;
using Infrastructure.Models.MarketplaceCategorySettings;
using Infrastructure.Models.MarketplaceProductSettings;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Stocks.Handlers;

public class
    GetMarketplaceStocksHandler : IRequestHandler<GetMarketplaceStocksRequest, IEnumerable<MarketplaceStockDto>>
{
    private readonly MContext _context;
    private readonly ILogger<GetMarketplaceStocksHandler> _logger;
    private readonly IMediator _mediator;

    public GetMarketplaceStocksHandler(MContext context, ILogger<GetMarketplaceStocksHandler> logger,
        IMediator mediator)
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IEnumerable<MarketplaceStockDto>> Handle(GetMarketplaceStocksRequest request,
        CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var result = new List<MarketplaceStockDto>();

        List<Product> products = await _context.StockChanges
            .AsNoTracking()
            .Where(s => s.MarketplaceId == request.MarketplaceId)
            .Include(s => s.Product)
            .ThenInclude(p => p.Category)
            .OrderBy(p => p.ProductId)
            .Take(request.Limit)
            .Select(c => c.Product)
            .ToListAsync(cancellationToken);

        if (products.Count == 0)
        {
            return result;
        }

        IEnumerable<int> productIds = products.Select(p => p.Id);
        IEnumerable<int> categoryIds = products
            .Where(p => p.CategoryId.HasValue).Select(p => p.CategoryId!.Value);

        Dictionary<int, MarketplaceProductSetting> productSettings = await _context.MarketplaceProductSettings
            .AsNoTracking()
            .Where(ps => ps.MarketplaceId == request.MarketplaceId && productIds.Contains(ps.ProductId))
            .ToDictionaryAsync(ps => ps.ProductId, ps => ps, cancellationToken);

        Dictionary<int, MarketplaceCategorySetting> categorySettings = await _context.MarketplaceCategorySettings
            .AsNoTracking()
            .Where(cs => cs.MarketplaceId == request.MarketplaceId && categoryIds.Contains(cs.CategoryId))
            .ToDictionaryAsync(cs => cs.CategoryId, cs => cs, cancellationToken);

        CompanyDto companyInformation = await _mediator.Send(new GetCompanyInformationRequest(), cancellationToken);
        Marketplace marketplace =
            await _context.Marketplaces.SingleAsync(m => m.Id == request.MarketplaceId, cancellationToken);

        Dictionary<int, decimal> stocks = await _context.Stocks
            .AsNoTracking()
            .Where(s => s.WarehouseId == marketplace.WarehouseId && productIds.Contains(s.ProductId))
            .ToDictionaryAsync(s => s.ProductId, s => s.Value, cancellationToken);

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
            categorySettings.TryGetValue(product.CategoryId ?? 0, out MarketplaceCategorySetting? categorySetting);

            string? externalId = marketplace.Type switch
            {
                MarketplaceType.Ozon => productSetting?.ExternalId,
                MarketplaceType.YandexMarket => productSetting?.ExternalId,
                _ => product.Articul
            };

            decimal value = GetStocks(product, productSetting, categorySetting);

            var stock = new MarketplaceStockDto
            {
                MarketplaceId = request.MarketplaceId,
                ProductId = product.Id,
                WarehouseId = marketplace.WarehouseId,
                ProductExternalId = externalId ?? string.Empty,
                Value = value
            };

            result.Add(stock);
        }

        stopwatch.Stop();

        _logger.LogInformation($"Handler stock request for " +
                               $"{request.MarketplaceId} with " +
                               $"{products.Count} elapsed " +
                               $"{stopwatch.ElapsedMilliseconds} ms");

        return result;

        decimal GetStocks(Product product, MarketplaceProductSetting? productSetting,
            MarketplaceCategorySetting? categorySetting)
        {
            if (product.Category is null || product.Category.DeletionMark)
            {
                return 0;
            }

            if (!stocks.TryGetValue(product.Id, out decimal value))
            {
                return 0;
            }

            if (value < marketplace.MinimalStock)
            {
                return 0;
            }

            decimal price = GetPrice(product);

            if (price == 0)
            {
                return 0;
            }

            if (productSetting is not null && productSetting.IgnoreRestrictions)
            {
                return value;
            }

            if (price < marketplace.MinimalPrice)
            {
                return 0;
            }

            if (productSetting is not null && productSetting.NullifyStock)
            {
                return 0;
            }

            if (categorySetting is not null && categorySetting.Blocked)
            {
                return 0;
            }

            return value;
        }

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