using Infrastructure;
using Infrastructure.Models.Marketplaces;
using Integration.SberMegaMarket.Services.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Common.Services.Orders;

public interface ILoadOrdersService
{
    Task LoadOrders();
}

public class LoadOrdersService : ILoadOrdersService
{
    private readonly ILogger<LoadOrdersService> _logger;
    private readonly MContext _context;
    private readonly IServiceProvider _serviceProvider;

    public LoadOrdersService(ILogger<LoadOrdersService> logger, MContext context, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public async Task LoadOrders()
    {
        try
        {
            List<Marketplace> marketplaces = await _context.Marketplaces
                .AsNoTracking()
                .Where(m => m.IsActive)
                .ToListAsync();

            foreach (Marketplace marketplace in marketplaces)
            {
                try
                {
                    MarketplaceOrderLoader? loader = marketplace.Type switch
                    {
                        MarketplaceType.SberMegaMarket => new SberMegaMarketOrderLoader(marketplace, _serviceProvider),
                        _ => null
                    };

                    if (loader is null)
                    {
                        continue;
                    }

                    await loader.LoadAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error while loading orders from {marketplace.Name}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading orders");
        }
    }
}