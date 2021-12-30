using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.Prices.Messages;

namespace Integration.Ozon.Clients.Prices;

public interface IOzonPriceClient
{
    Task SendPrices(OzonDto ozon, OzonPriceRequest request);
}

public class OzonPriceClient : BaseOzonClient, IOzonPriceClient
{
    public async Task SendPrices(OzonDto ozon, OzonPriceRequest request)
    {
        await PostAsync(ozon, "v1/product/import/prices", request);
    }
}