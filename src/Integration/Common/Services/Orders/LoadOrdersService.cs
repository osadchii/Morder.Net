using System.Diagnostics;
using AutoMapper;
using Infrastructure;
using Infrastructure.Models.Marketplaces;
using Integration.Ozon.Services.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Common.Services.Orders;

public interface ILoadOrdersService
{
    Task LoadOrders(DateTime startDate);
}

public class LoadOrdersService : ILoadOrdersService
{
    private readonly ILogger<LoadOrdersService> _logger;
    private readonly MContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;

    public LoadOrdersService(ILogger<LoadOrdersService> logger, MContext context, IServiceProvider serviceProvider,
        IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    public async Task LoadOrders(DateTime startDate)
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
                        MarketplaceType.Ozon => new OzonOrderLoader(marketplace, _serviceProvider, _mapper, startDate),
                        _ => null
                    };

                    if (loader is null)
                    {
                        continue;
                    }

                    var sw = new Stopwatch();
                    sw.Start();

                    await loader.LoadOrders();

                    sw.Stop();

                    _logger.LogInformation("Loaded {Marketplace} ({MarketplaceId}) orders. Elapsed {Ms} ms",
                        marketplace.Name, marketplace.Id, sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while loading orders from {Marketplace} ({MarketplaceId})",
                        marketplace.Name, marketplace.Id);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading orders");
        }
    }
}