using System.Diagnostics;
using AutoMapper;
using Infrastructure;
using Infrastructure.Models.Marketplaces;
using Integration.Kuper.Services.Orders;
using Integration.Ozon.Services.Orders;
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
            var marketplaces = await _context.Marketplaces
                .AsNoTracking()
                .Where(m => m.IsActive)
                .ToListAsync();

            foreach (var marketplace in marketplaces)
            {
                try
                {
                    MarketplaceOrderUpdater loader = marketplace.Type switch
                    {
                        MarketplaceType.SberMegaMarket => new SberMegaMarketOrderUpdater(marketplace, _serviceProvider,
                            _mapper),
                        MarketplaceType.Ozon => new OzonOrderUpdater(marketplace, _serviceProvider, _mapper),
                        MarketplaceType.Kuper => new KuperOrderUpdater(marketplace, _serviceProvider, _mapper),
                        _ => null
                    };

                    if (loader is null)
                    {
                        continue;
                    }

                    var sw = new Stopwatch();
                    sw.Start();
                    await loader.UpdateAsync();
                    sw.Stop();

                    _logger.LogInformation("{MarketplaceName} orders updated. Elapsed {Ms} ms", marketplace.Name,
                        sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while updating orders from {MarketplaceName}", marketplace.Name);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading orders");
        }
    }
}