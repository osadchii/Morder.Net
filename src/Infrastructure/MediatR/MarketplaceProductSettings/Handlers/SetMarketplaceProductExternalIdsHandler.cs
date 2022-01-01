using Infrastructure.MediatR.MarketplaceProductSettings.Commands;
using Infrastructure.Models.MarketplaceProductSettings;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.MarketplaceProductSettings.Handlers;

public class SetMarketplaceProductExternalIdsHandler : IRequestHandler<SetMarketplaceProductExternalIdsRequest, Unit>
{
    private readonly MContext _context;
    private readonly IChangeTrackingService _trackingService;

    public SetMarketplaceProductExternalIdsHandler(MContext context, IChangeTrackingService trackingService)
    {
        _context = context;
        _trackingService = trackingService;
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

        Dictionary<string, int> productIds = await _context.Products
            .AsNoTracking()
            .Where(p => articuls.Contains(p.Articul!))
            .Select(p => new { p.Articul, p.Id })
            .ToDictionaryAsync(p => p.Articul!, p => p.Id, cancellationToken);

        Dictionary<string, MarketplaceProductSetting> currentSettings = await _context.MarketplaceProductSettings
            .Where(s => s.MarketplaceId == request.MarketplaceId)
            .Include(s => s.Product)
            .ToDictionaryAsync(s => s.Product.Articul!, s => s, cancellationToken);

        foreach (KeyValuePair<string, string> keyValue in request.ExternalIds)
        {
            if (currentSettings.TryGetValue(keyValue.Key, out MarketplaceProductSetting? setting))
            {
                if (setting.ExternalId != keyValue.Value)
                {
                    setting.ExternalId = keyValue.Value;
                    await TrackChanges(setting.ProductId);
                }
            }
            else if (productIds.TryGetValue(keyValue.Key, out int productId))
            {
                setting = new MarketplaceProductSetting
                {
                    ExternalId = keyValue.Value,
                    MarketplaceId = request.MarketplaceId,
                    ProductId = productId
                };

                await _context.MarketplaceProductSettings.AddAsync(setting, cancellationToken);
                await TrackChanges(setting.ProductId);
            }
        }

        IEnumerable<KeyValuePair<string, MarketplaceProductSetting>> toClear =
            currentSettings.Where(s => !request.ExternalIds.ContainsKey(s.Key));

        foreach (KeyValuePair<string, MarketplaceProductSetting> clearItem in toClear)
        {
            clearItem.Value.ExternalId = null;
            await TrackChanges(clearItem.Value.ProductId);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

        async Task TrackChanges(int productId)
        {
            if (priceTracking)
            {
                await _trackingService.TrackPriceChange(request.MarketplaceId, productId, cancellationToken);
            }

            if (stockTracking)
            {
                await _trackingService.TrackStockChange(request.MarketplaceId, productId, cancellationToken);
            }
        }
    }
}