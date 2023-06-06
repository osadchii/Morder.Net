using AutoMapper;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Infrastructure.Models.Warehouses;
using Integration.Common.Services.StocksAndPrices.Stocks;
using Integration.Ozon.Clients.Stocks;
using Integration.Ozon.Clients.Stocks.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration.Ozon.Services;

public class OzonSendStockService : MarketplaceSendStockService
{
    public OzonSendStockService(IMediator mediator, IMapper mapper, IServiceProvider serviceProvider) : base(mediator,
        mapper, serviceProvider)
    {
    }

    public override async Task SendStocksAsync(Marketplace marketplace, MarketplaceStockDto[] stocks)
    {
        var client = ServiceProvider.GetRequiredService<IOzonStockClient>();
        var logger = ServiceProvider.GetRequiredService<ILogger<OzonSendStockService>>();
        var context = ServiceProvider.GetRequiredService<MContext>();
        
        var ozon = Mapper.Map<OzonDto>(marketplace);

        var warehouses = await context.OzonWarehouses
            .AsNoTracking()
            .Where(x => x.OzonId == ozon.Id)
            .Select(x => new {x.OzonWarehouseId, x.Id})
            .ToListAsync();

        var warehouseIds = warehouses.Select(x => x.Id);
        
        var blacklists = (await context.OzonWarehouseBlacklists
            .AsNoTracking()
            .Where(x => warehouseIds.Contains(x.OzonWarehouseId))
            .Select(x => new {x.OzonWarehouseId, x.ProductId})
            .ToListAsync())
            .GroupBy(x => x.OzonWarehouseId)
            .ToDictionary(x => x.Key, x => x
                .Select(bl => bl.ProductId)
                .ToHashSet());

        const int limit = 100;
        var requests = new List<OzonStockRequest>();
        var currentRequest = new OzonStockRequest
        {
            Stocks = new List<OzonStock>()
        };
        
        requests.Add(currentRequest);
        
        var stocksWithExternalId = stocks
            .Where(s => !s.ProductExternalId.IsNullOrEmpty());

        foreach (var stock in stocksWithExternalId)
        {
            if (!int.TryParse(stock.ProductExternalId, out var ozonProductId))
            {
                throw new Exception($"Wrong ozon external id: {stock.ProductExternalId} for product {stock.ProductId}");
            }

            var stockValue = Convert.ToInt32(stock.Value);
            
            var ozonStocks = warehouses.Select(warehouse =>
            {
                var warehouseStock = stockValue;
                if (blacklists.TryGetValue(warehouse.Id, out var blacklist) && blacklist.Contains(stock.ProductId))
                {
                    warehouseStock = 0;
                    logger.LogInformation("Product ${ProductId} is blacklisted for warehouse ${WarehouseId}",
                        stock.ProductId, warehouse.OzonWarehouseId);
                }
                return new OzonStock
                {
                    Stock = warehouseStock,
                    ProductId = ozonProductId,
                    WarehouseId = warehouse.OzonWarehouseId
                };
            });

            foreach (var ozonStock in ozonStocks)
            {
                currentRequest.Stocks.Add(ozonStock);

                if (currentRequest.Stocks.Count >= limit)
                {
                    currentRequest = new OzonStockRequest
                    {
                        Stocks = new List<OzonStock>()
                    };
                    requests.Add(currentRequest);
                }
            }
        }

        var emptyExternalIdCount = stocks.Count(p => p.ProductExternalId.IsNullOrEmpty());

        if (emptyExternalIdCount > 0)
        {
            logger.LogWarning("Found ${EmptyExternalIdCount} products with null or empty external id",
                emptyExternalIdCount);
        }

        var tasks = requests
            .Where(x => x.Stocks.Any())
            .Select(x => client.SendStocks(ozon, x));

        await Task.WhenAll(tasks);
    }
}