using Infrastructure.MediatR.MarketplaceProductSettings.Commands;
using Infrastructure.Models.MarketplaceProductSettings;
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
                    await _trackingService.TrackPriceChange(request.MarketplaceId, setting.ProductId,
                        cancellationToken);
                    await _trackingService.TrackStockChange(request.MarketplaceId, setting.ProductId,
                        cancellationToken);
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
                await _trackingService.TrackPriceChange(request.MarketplaceId, setting.ProductId,
                    cancellationToken);
                await _trackingService.TrackStockChange(request.MarketplaceId, setting.ProductId,
                    cancellationToken);
            }
        }

        IEnumerable<KeyValuePair<string, MarketplaceProductSetting>> toClear =
            currentSettings.Where(s => !request.ExternalIds.ContainsKey(s.Key));

        foreach (KeyValuePair<string, MarketplaceProductSetting> clearItem in toClear)
        {
            clearItem.Value.ExternalId = null;
            await _trackingService.TrackPriceChange(request.MarketplaceId, clearItem.Value.ProductId,
                cancellationToken);
            await _trackingService.TrackStockChange(request.MarketplaceId, clearItem.Value.ProductId,
                cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}