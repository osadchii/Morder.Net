using System.Globalization;
using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Infrastructure.Models.Prices;
using Integration.Common.Services.Prices;
using Integration.Ozon.Clients.Prices;
using Integration.Ozon.Clients.Prices.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Ozon.Services;

public class OzonSendPriceService : MarketplaceSendPriceService
{
    public OzonSendPriceService(IMapper mapper, IServiceProvider serviceProvider) : base(mapper, serviceProvider)
    {
    }

    public override async Task SendPricesAsync(Marketplace marketplace, IEnumerable<MarketplacePriceDto> prices)
    {
        var client = ServiceProvider.GetRequiredService<IOzonPriceClient>();
        var ozon = Mapper.Map<OzonDto>(marketplace);
        var request = new OzonPriceRequest()
        {
            Prices = prices.Select(s =>
            {
                if (!int.TryParse(s.ProductExternalId, out int productId))
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

        await client.SendPrices(ozon, request);
    }
}