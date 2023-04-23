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
        var request = new OzonStockRequest
        {
            Stocks = stocks
                .Where(s => !s.ProductExternalId.IsNullOrEmpty()).SelectMany(s =>
                {
                    if (!int.TryParse(s.ProductExternalId, out var productId))
                    {
                        throw new Exception($"Wrong ozon external id: {s.ProductExternalId} for product {s.ProductId}");
                    }

                    return ozon.Settings.WarehouseIds.Select(warehouse => new OzonStock
                    {
                        Stock = Convert.ToInt32(s.Value),
                        ProductId = productId,
                        WarehouseId = warehouse
                    });
                })
        };

        var emptyExternalIdCount = stocks.Count(p => p.ProductExternalId.IsNullOrEmpty());

        if (emptyExternalIdCount > 0)
        {
            logger.LogWarning("Found ${EmptyExternalIdCount} products with null or empty external id",
                emptyExternalIdCount);
        }

        if (request.Stocks.Any())
        {
            await client.SendStocks(ozon, request);
        }
    }
}