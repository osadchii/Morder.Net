using System.Globalization;
using AutoMapper;
using Infrastructure;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Kuper;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using Integration.Common.Services.StocksAndPrices.Prices;
using Integration.Kuper.Clients.Prices;
using Integration.Kuper.Clients.Prices.Messages;
using Integration.Kuper.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Price = Integration.Kuper.Clients.Prices.Messages.Price;

namespace Integration.Kuper.Services;

public class KuperSendPriceService : MarketplaceSendPriceService
{
    public KuperSendPriceService(IMapper mapper, IServiceProvider serviceProvider) : base(mapper, serviceProvider)
    {
    }

    public override async Task SendPricesAsync(Marketplace marketplace, MarketplacePriceDto[] prices)
    {
        var client = ServiceProvider.GetRequiredService<IKuperPriceClient>();
        var kuper = Mapper.Map<KuperDto>(marketplace);
        var logger = ServiceProvider.GetRequiredService<ILogger<KuperSendPriceService>>();
        var context = ServiceProvider.GetRequiredService<MContext>();
        var productImageService = ServiceProvider.GetRequiredService<IProductImageService>();

        var productIdsWithImages = await productImageService.GetProductIdsWithImages();

        var vatByArticul = await context.Products
            .AsNoTracking()
            .Where(x => prices.Select(p => p.Articul).Contains(x.Articul))
            .ToDictionaryAsync(x => x.Articul, x => x.Vat);

        var message = new KuperPriceMessage
        {
            Data = prices
                .Where(x => productIdsWithImages.Contains(x.ProductId))
                .Select(x =>
                {
                    vatByArticul.TryGetValue(x.Articul, out var vatValue);

                    return new Data
                    {
                        Active = true,
                        OfferId = x.Articul,
                        Price = new Price
                        {
                            Currency = "RUB",
                            Amount = x.Value.ToString(CultureInfo.InvariantCulture)
                        },
                        PriceType = "BASE",
                        PriceCategory = "ONLINE",
                        OutletId = kuper.WarehouseExternalId.ToString(),
                        Vat = vatValue.ConvertToString()
                    };
                })
                .ToArray()
        };

        if (message.Data.Length == 0)
        {
            logger.LogInformation("No prices to send");
            return;
        }

        await client.SendPrices(kuper, message);
        logger.LogInformation("Sent {Count} prices to Kuper", message.Data.Length);
    }
}