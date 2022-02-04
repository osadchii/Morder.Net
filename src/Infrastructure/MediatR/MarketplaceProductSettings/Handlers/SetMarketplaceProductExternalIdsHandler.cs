using Infrastructure.MediatR.MarketplaceProductSettings.Commands;
using Infrastructure.Models.MarketplaceProductSettings;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.MarketplaceProductSettings.Handlers;

public class SetMarketplaceProductExternalIdsHandler : IRequestHandler<SetMarketplaceProductExternalIdsRequest, Unit>
{
    private readonly MContext _context;
    private readonly IChangeTrackingService _trackingService;
    private readonly ILogger<SetMarketplaceProductExternalIdsHandler> _logger;

    public SetMarketplaceProductExternalIdsHandler(MContext context, IChangeTrackingService trackingService,
        ILogger<SetMarketplaceProductExternalIdsHandler> logger)
    {
        _context = context;
        _trackingService = trackingService;
        _logger = logger;
    }

    public async Task<Unit> Handle(SetMarketplaceProductExternalIdsRequest request, CancellationToken cancellationToken)
    {
        List<string> articuls = request.ExternalIds.Keys.ToList();
        Marketplace marketplace = await _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.Id == request.MarketplaceId)
            .SingleAsync(cancellationToken);

        bool stockTracking = marketplace.IsActive && marketplace.StockChangesTracking;
        bool priceTracking = marketplace.IsActive && marketplace.PriceChangesTracking;

        var newProductIds = new List<int>();

        Dictionary<string, int> productsData = await _context.Products
            .AsNoTracking()
            .Where(p => articuls.Contains(p.Articul!))
            .Select(p => new { p.Articul, p.Id })
            .ToDictionaryAsync(p => p.Articul!, p => p.Id, cancellationToken);

        Dictionary<string, MarketplaceProductSetting> currentSettings = await _context.MarketplaceProductSettings
            .Where(s => s.MarketplaceId == request.MarketplaceId)
            .Include(s => s.Product)
            .ToDictionaryAsync(s => s.Product.Articul!, s => s, cancellationToken);

        foreach ((string? articul, string? value) in request.ExternalIds)
        {
            if (currentSettings.TryGetValue(articul, out MarketplaceProductSetting? setting))
            {
                if (setting.ExternalId != value)
                {
                    setting.ExternalId = value;
                    newProductIds.Add(setting.ProductId);
                    _logger.LogInformation(
                        "Product with articul {Articul} mapped to {ExternalId} in marketplace with id {Id}",
                        articul, value, request.MarketplaceId);
                }
            }
            else if (productsData.TryGetValue(articul, out int productId))
            {
                setting = new MarketplaceProductSetting
                {
                    ExternalId = value,
                    MarketplaceId = request.MarketplaceId,
                    ProductId = productId
                };

                await _context.MarketplaceProductSettings.AddAsync(setting, cancellationToken);
                newProductIds.Add(setting.ProductId);
                _logger.LogInformation(
                    "Product with articul {Articul} mapped to {ExternalId} in marketplace with id {Id}",
                    articul, value, request.MarketplaceId);
            }
        }

        string[] notFoundArticuls = articuls.Except(productsData.Keys.ToList()).ToArray();

        if (notFoundArticuls.Any())
        {
            _logger.LogWarning(
                "Can't find products with articuls {Articuls} to set {MarketplaceName} ({MarketplaceId}) external ids",
                string.Join(", ", notFoundArticuls), marketplace.Name, marketplace.Id);
        }

        IEnumerable<KeyValuePair<string, MarketplaceProductSetting>> toClear =
            currentSettings.Where(s => !request.ExternalIds.ContainsKey(s.Key));

        foreach ((_, MarketplaceProductSetting? value) in toClear)
        {
            value.ExternalId = null;
        }

        await _context.SaveChangesAsync(cancellationToken);

        if (priceTracking)
        {
            await _trackingService.TrackPricesChange(request.MarketplaceId, newProductIds, cancellationToken);
        }

        if (stockTracking)
        {
            await _trackingService.TrackStocksChange(request.MarketplaceId, newProductIds, cancellationToken);
        }

        return Unit.Value;
    }
}