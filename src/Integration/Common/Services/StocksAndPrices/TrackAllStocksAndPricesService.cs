using Infrastructure;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Services.Marketplaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Common.Services.StocksAndPrices;

public interface ITrackAllStocksAndPricesService
{
    Task TrackAllStocksAndPrices();
}

public class TrackAllStocksAndPricesService : ITrackAllStocksAndPricesService
{
    private readonly IChangeTrackingService _changeTrackingService;
    private readonly ILogger<TrackAllStocksAndPricesService> _logger;
    private readonly MContext _context;

    public TrackAllStocksAndPricesService(IChangeTrackingService changeTrackingService, ILogger<TrackAllStocksAndPricesService> logger, MContext context)
    {
        _changeTrackingService = changeTrackingService;
        _logger = logger;
        _context = context;
    }

    public async Task TrackAllStocksAndPrices()
    {
        try
        {
            List<Marketplace> marketplaces = await _context.Marketplaces
                .AsNoTracking()
                .Where(m => m.IsActive )
                .ToListAsync();

            foreach (Marketplace marketplace in marketplaces)
            {
                try
                {
                    if (marketplace.PriceChangesTracking)
                    {
                        await _changeTrackingService.TrackAllPrices(marketplace.Id, CancellationToken.None);
                        _logger.LogInformation("All prices have been tracked for marketplace ({MarketplaceName}) with {MarketplaceId}", marketplace.Name, marketplace.Id);
                    }

                    if (marketplace.StockChangesTracking)
                    {
                        await _changeTrackingService.TrackAllStocks(marketplace.Id, CancellationToken.None);
                        _logger.LogInformation("All stocks have been tracked for marketplace ({MarketplaceName}) with {MarketplaceId}", marketplace.Name, marketplace.Id);
                    }
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while sending prices to {MarketplaceName}", marketplace.Name);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while tracking stocks and prices");
        }
    }
}