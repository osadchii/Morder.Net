using System.Globalization;
using AutoMapper;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Infrastructure.Models.Prices;
using Integration.Common.Services.StocksAndPrices.Prices;
using Integration.Ozon.Clients.Prices;
using Integration.Ozon.Clients.Prices.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration.Ozon.Services;

public class OzonSendPriceService : MarketplaceSendPriceService
{
    public OzonSendPriceService(IMapper mapper, IServiceProvider serviceProvider) : base(mapper, serviceProvider)
    {
    }

    public override async Task SendPricesAsync(Marketplace marketplace, MarketplacePriceDto[] prices)
    {
        var client = ServiceProvider.GetRequiredService<IOzonPriceClient>();
        var ozon = Mapper.Map<OzonDto>(marketplace);
        var logger = ServiceProvider.GetRequiredService<ILogger<OzonSendPriceService>>();

        var request = new OzonPriceRequest()
        {
            Prices = prices
                .Where(p => !p.ProductExternalId.IsNullOrEmpty())
                .Select(s =>
                {
                    if (!int.TryParse(s.ProductExternalId, out var productId))
                    {
                        throw new Exception($"Wrong ozon external id: {s.ProductExternalId} for product {s.ProductId}");
                    }

                    return new OzonPrice
                    {
                        Price = s.Value.ToString(CultureInfo.InvariantCulture),
                        ProductId = productId
                    };
                })
        };

        if (!request.Prices.Any())
        {
            return;
        }

        var emptyExternalIdCount = prices.Count(p => p.ProductExternalId.IsNullOrEmpty());

        if (emptyExternalIdCount > 0)
        {
            logger.LogWarning($"Found ${emptyExternalIdCount} products with null or empty external id");
        }

        await client.SendPrices(ozon, request);
    }
}