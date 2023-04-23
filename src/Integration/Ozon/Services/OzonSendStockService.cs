using AutoMapper;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Infrastructure.Models.Warehouses;
using Integration.Common.Services.StocksAndPrices.Stocks;
using Integration.Ozon.Clients.Stocks;
using Integration.Ozon.Clients.Stocks.Messages;
using MediatR;
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
        var ozon = Mapper.Map<OzonDto>(marketplace);

        const int limit = 100;
        var requests = new List<OzonStockRequest>();
        var currentRequest = new OzonStockRequest
        {
            Stocks = new List<OzonStock>()
        };
        
        requests.Add(currentRequest);
        
        IEnumerable<MarketplaceStockDto> stocksWithExternalId = stocks
            .Where(s => !s.ProductExternalId.IsNullOrEmpty());

        foreach (MarketplaceStockDto stock in stocksWithExternalId)
        {
            if (!int.TryParse(stock.ProductExternalId, out var productId))
            {
                throw new Exception($"Wrong ozon external id: {stock.ProductExternalId} for product {stock.ProductId}");
            }

            var stockValue = Convert.ToInt32(stock.Value);
            IEnumerable<OzonStock> ozonStocks = ozon.Settings.WarehouseIds.Select(warehouseId => new OzonStock
            {
                Stock = stockValue,
                ProductId = productId,
                WarehouseId = warehouseId
            });

            foreach (OzonStock ozonStock in ozonStocks)
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

        IEnumerable<Task> tasks = requests
            .Where(x => x.Stocks.Any())
            .Select(x => client.SendStocks(ozon, x));

        await Task.WhenAll(tasks);
    }
}