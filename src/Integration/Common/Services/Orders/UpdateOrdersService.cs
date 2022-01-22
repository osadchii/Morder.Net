using AutoMapper;
using Infrastructure;
using Infrastructure.Models.Marketplaces;
using Integration.Ozon.Services;
using Integration.SberMegaMarket.Services.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Common.Services.Orders;

public interface IUpdateOrdersService
{
    Task UpdateOrders();
}

public class UpdateOrdersService : IUpdateOrdersService
{
    private readonly ILogger<UpdateOrdersService> _logger;
    private readonly MContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;

    public UpdateOrdersService(ILogger<UpdateOrdersService> logger, MContext context, IServiceProvider serviceProvider,
        IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    public async Task UpdateOrders()
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
                    MarketplaceOrderUpdater? loader = marketplace.Type switch
                    {
                        MarketplaceType.SberMegaMarket => new SberMegaMarketOrderUpdater(marketplace, _serviceProvider,
                            _mapper),
                        MarketplaceType.Ozon => new OzonOrderUpdater(marketplace, _serviceProvider, _mapper),
                        _ => null
                    };

                    if (loader is null)
                    {
                        continue;
                    }

                    await loader.UpdateAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error while updating orders from {marketplace.Name}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading orders");
        }
    }
}