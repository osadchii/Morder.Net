using System.Globalization;
using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Kuper;
using Infrastructure.Models.Warehouses;
using Infrastructure.Services.Marketplaces;
using Integration.Common.Services.StocksAndPrices.Stocks;
using Integration.Kuper.Clients.Stocks;
using Integration.Kuper.Clients.Stocks.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration.Kuper.Services;

public class KuperSendStockService : MarketplaceSendStockService
{
    public KuperSendStockService(IMediator mediator, IMapper mapper, IServiceProvider serviceProvider) : base(mediator,
        mapper, serviceProvider)
    {
    }

    public override async Task SendStocksAsync(Marketplace marketplace, MarketplaceStockDto[] stocks)
    {
        var client = ServiceProvider.GetRequiredService<IKuperStockClient>();
        var productImageService = ServiceProvider.GetRequiredService<IProductImageService>();
        var logger = ServiceProvider.GetRequiredService<ILogger<KuperSendStockService>>();

        var kuper = Mapper.Map<KuperDto>(marketplace);

        var productIdsWithImages = await productImageService.GetProductIdsWithImages();

        var message = new KuperStockMessage
        {
            Data = stocks
                .Where(x => productIdsWithImages.Contains(x.ProductId))
                .Select(x => new Data
                {
                    OfferId = x.Articul,
                    OutletId = kuper.WarehouseExternalId.ToString(),
                    Stock = x.Value.ToString(CultureInfo.InvariantCulture)
                }).ToArray()
        };

        if (message.Data.Length == 0)
        {
            logger.LogInformation("No stocks to send");
            return;
        }

        await client.SendStocks(kuper, message);

        logger.LogInformation("Sent {Count} stocks to Kuper", message.Data.Length);
    }
}