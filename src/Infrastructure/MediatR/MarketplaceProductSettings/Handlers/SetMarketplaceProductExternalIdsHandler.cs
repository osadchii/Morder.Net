using Infrastructure.MediatR.MarketplaceProductSettings.Commands;
using Infrastructure.Models.MarketplaceProductSettings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.MarketplaceProductSettings.Handlers;

public class SetMarketplaceProductExternalIdsHandler : IRequestHandler<SetMarketplaceProductExternalIdsRequest, Unit>
{
    private readonly MContext _context;

    public SetMarketplaceProductExternalIdsHandler(MContext context)
    {
        _context = context;
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
                setting.ExternalId = keyValue.Value;
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
            }
        }

        IEnumerable<KeyValuePair<string, MarketplaceProductSetting>> toClear =
            currentSettings.Where(s => !request.ExternalIds.ContainsKey(s.Key));

        foreach (KeyValuePair<string, MarketplaceProductSetting> clearItem in toClear)
        {
            clearItem.Value.ExternalId = null;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}